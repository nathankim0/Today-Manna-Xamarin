using System;
using System.IO;
using Xamarin.Forms;
using TodaysManna.Services;
using System.Diagnostics;
using Xamarin.Essentials;
using TodaysManna.Constants;

namespace TodaysManna.ViewModel
{
    public class SettingViewModel : PageBaseViewModel
    {
        public DropBoxService saveDropBoxService;
        public DropBoxService restoreDropBoxService;
        public DropBoxService getMetadataDropBoxService;

        private string _version;
        public string Version { get => _version; set => SetProperty(ref _version, value); }

        private string _isDatabase;
        public string IsDatabase { get => _isDatabase; set => SetProperty(ref _isDatabase, value); }

        public SettingViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Title = "설정";

            saveDropBoxService = new DropBoxService();
            restoreDropBoxService = new DropBoxService();
            getMetadataDropBoxService = new DropBoxService();

            saveDropBoxService.OnAuthenticated += BackupDatabase;
            restoreDropBoxService.OnAuthenticated += RestoreDatabase;
            getMetadataDropBoxService.OnAuthenticated += GetIsDatabase;

            saveDropBoxService.OnDisappeared += ChangeBusyStatus;
            restoreDropBoxService.OnDisappeared += ChangeBusyStatus;
            getMetadataDropBoxService.OnDisappeared += ChangeBusyStatus;

            VersionTracking.Track();
            Version = VersionTracking.CurrentVersion;
        }

        private void ChangeBusyStatus()
        {
            IsBusy = false;
        }

        public void GetAuthAndIsDatabase()
        {
            getMetadataDropBoxService.CheckAuth();
        }

        public async void GetIsDatabase()
        {
            try
            {
                // Read the database from app storage
                var dbMetadata = await getMetadataDropBoxService.GetMetadata($"/{Rests.DatabaseFilename}");
                if (dbMetadata != null)
                {
                    var createDateTime = dbMetadata.AsFile.ServerModified;
                    var kstTime = createDateTime.ToLocalTime();
                    IsDatabase = kstTime.ToString("f");
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

        private async void BackupDatabase()
        {
            try
            {
                // Read the database from app storage
                byte[] db = File.ReadAllBytes(Rests.DatabasePath);

                // Write the database to DropBox folder
                var isSuccess = await saveDropBoxService.WriteFile(db, $"/{Rests.DatabaseFilename}");
                if (isSuccess != null)
                {
                    //await Application.Current.MainPage.DisplayAlert("Drobpox", "백업 성공!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Dropbox", $"백업 실패, 다시 시도해보세요.", "확인");
                }
                GetIsDatabase();
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
                var db = await restoreDropBoxService.ReadFile($"/{Rests.DatabaseFilename}");

                if (db != null)
                {
                    // Write the database to storage
                    File.WriteAllBytes(Rests.DatabasePath, db);

                    //await Application.Current.MainPage.DisplayAlert("Drobpox", "복원 성공!", "확인");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Dropbox", $"복원 실패, 다시 시도해보세요.", "확인");
                }
                GetIsDatabase();
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

        public void Logout()
        {
            try
            {
                Application.Current.Properties.Clear();
                DependencyService.Get<IClearCookies>().Clear();

                saveDropBoxService = new DropBoxService();
                restoreDropBoxService = new DropBoxService();
                getMetadataDropBoxService = new DropBoxService();

                saveDropBoxService.OnAuthenticated += BackupDatabase;
                restoreDropBoxService.OnAuthenticated += RestoreDatabase;
                getMetadataDropBoxService.OnAuthenticated += GetIsDatabase;

                saveDropBoxService.OnDisappeared += ChangeBusyStatus;
                restoreDropBoxService.OnDisappeared += ChangeBusyStatus;
                getMetadataDropBoxService.OnDisappeared += ChangeBusyStatus;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
                IsDatabase = "";
            }
        }
    }
}
