using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;
using TodaysManna.Services;
using System.Diagnostics;
using Xamarin.Essentials;
using Plugin.StoreReview;

namespace TodaysManna.ViewModel
{
    public class SettingViewModel : PageBaseViewModel
    {
        private DropBoxService saveDropBoxService;
        private DropBoxService restoreDropBoxService;
        private DropBoxService getMetadataDropBoxService;

        public ICommand SaveCommand => new Command(OnSaveButtonClicked);
        public ICommand RestoreCommand => new Command(OnRestoreButtonClicked);
        public ICommand OpenStoreCommand => new Command(OnOpenStoreButtonClicked);
        public ICommand CopyReportEmailCommand => new Command(OnReportButtonClicked);
        public ICommand DonateCommand => new Command(OnDonateButtonClicked);

        public SettingViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "설정";

            saveDropBoxService = new DropBoxService();
            restoreDropBoxService = new DropBoxService();
            getMetadataDropBoxService = new DropBoxService();

            saveDropBoxService.OnAuthenticated += SaveDatabase;
            restoreDropBoxService.OnAuthenticated += RestoreDatabase;
            getMetadataDropBoxService.OnAuthenticated += GetIsDatabase;

            saveDropBoxService.OnDisappeared += ChangeBusyStatus;
            restoreDropBoxService.OnDisappeared += ChangeBusyStatus;
            getMetadataDropBoxService.OnDisappeared += ChangeBusyStatus;

            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;
        }

        private async void OnReportButtonClicked()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_report");

            var address = "jinyeob07@gmail.com";
            await Clipboard.SetTextAsync(address);
            await Application.Current.MainPage.DisplayAlert("클립보드에 복사됨", address, "확인");
        }

        private async void OnDonateButtonClicked()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_donate");

            var uri = new Uri("https://qr.kakaopay.com/281006011000037630355680");
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        private void ChangeBusyStatus()
        {
            IsBusy = false;
        }

        private string _isDatabase;
        public string IsDatabase { get => _isDatabase; set => SetProperty(ref _isDatabase, value); }

        public void GetAuthAndIsDatabase()
        {
            getMetadataDropBoxService.CheckAuth();
        }

        public async void GetIsDatabase()
        {
            try
            {
                // Read the database from app storage
                var dbMetadata = await getMetadataDropBoxService.GetMetadata($"/{Constants.DatabaseFilename}");
                if (dbMetadata != null)
                {
                    IsDatabase = dbMetadata.AsFile.ServerModified.ToString("f");
                }
                else
                {
                    IsDatabase = "";
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private string _version;
        public string Version { get => _version; set => SetProperty(ref _version, value); }

        public UriImageSource DropboxIcon => new UriImageSource
        {
            Uri = new Uri(
                    new OnPlatform<string>
                    {
                        iOS = "https://www.dropbox.com/sh/zcxbhhco5x0bwmm/AAD6Nq7vmOBalRfOOe9oIa_va?dl=1",
                        Android = "https://www.dropbox.com/s/0l0fpn9yytzb1t7/dropbox-android.png?dl=1"
                    })
        };

        private async void OnSaveButtonClicked()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_backup");

            IsBusy = true;

            await saveDropBoxService.Authorize();
        }

        private async void OnRestoreButtonClicked()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_restore");

            if (!await Application.Current.MainPage.DisplayAlert("", "복원하시겠습니까? 기존 메모는 사라집니다.", "확인", "취소"))
            {
                IsBusy = false;

                return;
            }
            IsBusy = true;

            await restoreDropBoxService.Authorize();
        }

        private async void OnOpenStoreButtonClicked()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("setting_review");

            //CrossStoreReview.Current.OpenStoreListing("1547824358");
            //CrossStoreReview.Current.OpenStoreReviewPage("1547824358");
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    await CrossStoreReview.Current.RequestReview(false);
                    break;
                case Device.Android:
                    var uri = new Uri("https://play.google.com/store/apps/details?id=com.manna.parsing2");
                    await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
                    break;
                default:
                    break;
            }
        }

        private async void SaveDatabase()
        {
            try
            {
                // Read the database from app storage
                byte[] db = File.ReadAllBytes(Constants.DatabasePath);

                // Write the database to DropBox folder
                var isSuccess = await saveDropBoxService.WriteFile(db, $"/{Constants.DatabaseFilename}");
                if (isSuccess != null)
                {
                    //await Application.Current.MainPage.DisplayAlert("Drobpox", "백업 성공!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Dropbox", $"백업 실패, 다시 시도해보세요.", "확인");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Dropbox", $"Something went wrong. If the problem persists, please contact us", "OK");
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void RestoreDatabase()
        {
            try
            {
                // Read the database from DropBox folder
                var db = await restoreDropBoxService.ReadFile($"/{Constants.DatabaseFilename}");

                if (db != null)
                {
                    // Write the database to storage
                    File.WriteAllBytes(Constants.DatabasePath, db);

                    //await Application.Current.MainPage.DisplayAlert("Drobpox", "복원 성공!", "확인");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Dropbox", $"복원 실패, 다시 시도해보세요.", "확인");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Application.Current.MainPage.DisplayAlert("Dropbox", $"Something went wrong. If the problem persists, please contact us", "OK");
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
