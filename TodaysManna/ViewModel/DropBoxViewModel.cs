//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using Xamarin.Forms;
//using TodaysManna.Services;
//using System.Diagnostics;

//namespace TodaysManna.ViewModel
//{
//    /// <summary>
//    ///     Implements DropBoxService
//    /// </summary>
//    public class DropBoxViewModel
//    {
//        #region Fields

//        private readonly DropBoxService dropBoxService = new DropBoxService();

//        #endregion

//        #region Constructors and Destructors

//        public DropBoxViewModel()
//        {
//            this.DropBoxGetCommand = new Command(this.OnDropboxGet);
//            this.DropBoxSaveCommand = new Command(this.OnDropboxSave);
//        }

//        #endregion

//        #region Public Properties

//        public ICommand DropBoxGetCommand { get; }

//        public ICommand DropBoxSaveCommand { get; }

//        public UriImageSource DropboxIcon
//            =>
//            new UriImageSource
//            {
//                Uri =
//                        new Uri(
//                            new OnPlatform<string>
//                            {
//                                iOS = "https://www.dropbox.com/sh/zcxbhhco5x0bwmm/AAD6Nq7vmOBalRfOOe9oIa_va?dl=1",
//                                Android = "https://www.dropbox.com/s/0l0fpn9yytzb1t7/dropbox-android.png?dl=1"
//                            })
//            };

//        #endregion

//        #region Methods

//        private async void LoadDatabase()
//        {
//            try
//            {
//                // Read the database from DropBox folder
//                var db = await this.dropBoxService.ReadFile("DropBoxDatabasePath");

//                // Write the database to storage
//                //await this.sqLite.WriteDatabase(db);

//                await Application.Current.MainPage.DisplayAlert("Drobpox", "Loaded database OK!", "OK");

//                // Go back to origin
//                // await this.NavigationService.GoBackAsync();
//            }
//            catch (Exception ex)
//            {
//            }
//        }

//        private async void OnDropboxGet()
//        {
//            // If the user authenticates - loads database to Dropbox
//            this.dropBoxService.OnAuthenticated += this.LoadDatabase;
//            await this.dropBoxService.Authorize();
//        }

//        private async void OnDropboxSave()
//        {
//            // If the user authenticates - save database to Dropbox
//            this.dropBoxService.OnAuthenticated += this.SaveDatabase;
//            await this.dropBoxService.Authorize();
//        }

//        private async void SaveDatabase()
//        {
//            try
//            {
//                // Read the database from app storage
//                //var db = this.sqLite.ReadDatabase();

//                // Write the database to DropBox folder
//                //await this.dropBoxService.WriteFile(db, "DropBoxDatabasePath");

//                await Application.Current.MainPage.DisplayAlert("Drobpox", "Saved database OK!", "OK");
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine(ex);
//                await Application.Current.MainPage.DisplayAlert("Dropbox", $"Something went wrong. If the problem persists, please contact us", "OK");
//            }
//        }

//        #endregion
//    }
//}
