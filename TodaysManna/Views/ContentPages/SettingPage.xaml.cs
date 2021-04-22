using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using TodaysManna.Services;
using Xamarin.Forms;

namespace TodaysManna.Views
{
    public partial class SettingPage : ContentPage
    {
        DropBoxService dropBoxService;
         
        public SettingPage()
        {
            InitializeComponent();

            dropBoxService = new DropBoxService();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        { 
            await dropBoxService.Authorize();
            SaveDatabase();
        }

        private async void SaveDatabase()
        {
            try
            {
                // Read the database from app storage
                byte[] db = File.ReadAllBytes(Constants.DatabasePath);

                // Write the database to DropBox folder
                var isSuccess = await dropBoxService.WriteFile(db, $"/{Constants.DatabaseFilename}");
                if (isSuccess != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Drobpox", "백업 성공!", "OK");
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
            }
        }

        async void Button_Clicked_1(object sender, EventArgs e)
        {
            await dropBoxService.Authorize();
            RestoreDatabase();
        }

        private async void RestoreDatabase()
        {
            try
            {
                // Read the database from DropBox folder
                var db = await dropBoxService.ReadFile($"/{Constants.DatabaseFilename}");

                if (db != null)
                {
                    // Write the database to storage
                    File.WriteAllBytes(Constants.DatabasePath, db);

                    await Application.Current.MainPage.DisplayAlert("Drobpox", "복원 성공!", "확인");
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
            }
        }
    }
}
