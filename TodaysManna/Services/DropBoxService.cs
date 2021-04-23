using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Dropbox.Api;
using Dropbox.Api.Files;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Application = Xamarin.Forms.Application;

namespace TodaysManna.Services
{
    public class DropBoxService
    {
        #region Constants

        private const string AppKeyDropboxtoken = "sl.AvZsfCZJLdH8B4ir5tXbgCZqp9hPkmwo9c5qZlPysyn2-cv_nNcPJGe4CtNOI2C8vaiQPH5B8E_rQcIjSRFMJ30raC5gQVEPEjNbzCc2TBwgR2-NG344iI4RggqE4V961Ibk1SyVGHM";
        private const string ClientId = "xdkji9g6jlvv4u1";
        private const string RedirectUri = "https://localhost/TodaysManna";

        #endregion

        #region Fields

        /// <summary>
        ///     Occurs when the user was authenticated
        /// </summary>
        public Action OnAuthenticated;
        public Action OnDisappeared;

        private string oauth2State;

        #endregion

        #region Properties

        private string AccessToken { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     <para>Runs the Dropbox OAuth authorization process if not yet authenticated.</para>
        ///     <para>Upon completion <seealso cref="OnAuthenticated"/> is called</para>
        /// </summary>
        /// <returns>An asynchronous task.</returns>
        public async Task Authorize()
        {
            if (string.IsNullOrWhiteSpace(this.AccessToken) == false)
            {
                // Already authorized
                this.OnAuthenticated?.Invoke();
                return;
            }

            if (this.GetAccessTokenFromSettings())
            {
                // Found token and set AccessToken 
                return;
            }

            // Run Dropbox authentication
            this.oauth2State = Guid.NewGuid().ToString("N");
            var authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, ClientId, new Uri(RedirectUri), this.oauth2State);
            var webView = new WebView { Source = new UrlWebViewSource { Url = authorizeUri.AbsoluteUri } };
            webView.Navigating += this.WebViewOnNavigating;
            var contentPage = new WebViewPage { Content = webView };
            contentPage.Disappearing += ContentPage_Disappearing;
            await Application.Current.MainPage.Navigation.PushModalAsync(contentPage);
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            OnDisappeared?.Invoke();
        }

        public void CheckAuth()
        {
            if (string.IsNullOrWhiteSpace(this.AccessToken) == false)
            {
                // Already authorized
                this.OnAuthenticated?.Invoke();
                return;
            }

            if (this.GetAccessTokenFromSettings())
            {
                // Found token and set AccessToken 
                return;
            }
        }

        public class WebViewPage : ContentPage
        {
            public WebViewPage()
            {
                On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
            }
        }

        public async Task<IList<Metadata>> ListFiles()
        {
            try
            {
                using (var client = this.GetClient())
                {
                    var list = await client.Files.ListFolderAsync(string.Empty);
                    return list?.Entries;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<Metadata> GetMetadata(string file)
        {
            try
            {
                using (var client = this.GetClient())
                {
                    Metadata metadata = await client.Files.GetMetadataAsync(file);
                    return metadata;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<byte[]> ReadFile(string file)
        {
            try
            {
                using (var client = this.GetClient())
                {                    
                    var response = await client.Files.DownloadAsync(file);
                    var bytes = response?.GetContentAsByteArrayAsync();
                    return bytes?.Result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }



        public async Task<FileMetadata> WriteFile(byte[] fileContent, string filename)
        {
            try
            {
                var commitInfo = new CommitInfo(filename, WriteMode.Overwrite.Instance, false, DateTime.Now);

                using (var client = this.GetClient())
                {
                    var metadata = await client.Files.UploadAsync(commitInfo, new MemoryStream(fileContent));
                    return metadata;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Saves the Dropbox token to app settings
        /// </summary>
        /// <param name="token">Token received from Dropbox authentication</param>
        private static async Task SaveDropboxToken(string token)
        {
            if (token == null)
            {
                return;
            }

            try
            {
                Application.Current.Properties.Add(AppKeyDropboxtoken, token);
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private DropboxClient GetClient()
        {
            if(Device.RuntimePlatform == Device.Android)
            {
                return new DropboxClient(AccessToken, new DropboxClientConfig() { HttpClient = new HttpClient(new HttpClientHandler()) });
            }
            else
            {
                return new DropboxClient(AccessToken);
            }
        }

        /// <summary>
        ///     Tries to find the Dropbox token in application settings
        /// </summary>
        /// <returns>Token as string or <c>null</c></returns>
        private bool GetAccessTokenFromSettings()
        {
            try
            {
                if (!Application.Current.Properties.ContainsKey(AppKeyDropboxtoken))
                {
                    return false;
                }

                this.AccessToken = Application.Current.Properties[AppKeyDropboxtoken]?.ToString();
                if (this.AccessToken != null)
                {
                    this.OnAuthenticated?.Invoke();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        private async void WebViewOnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (!e.Url.StartsWith(RedirectUri, StringComparison.OrdinalIgnoreCase))
            {
                // we need to ignore all navigation that isn't to the redirect uri.
                return;
            }

            try
            {
                var result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(e.Url));

                if (result.State != this.oauth2State)
                {
                    return;
                }

                this.AccessToken = result.AccessToken;

                await SaveDropboxToken(this.AccessToken);
                this.OnAuthenticated?.Invoke();
            }
            catch (ArgumentException)
            {
                // There was an error in the URI passed to ParseTokenFragment
            }
            finally
            {
                e.Cancel = true;
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        #endregion

    }
}