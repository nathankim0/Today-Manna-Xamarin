using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using TodaysManna.Datas;
using TodaysManna.Popups;
using TodaysManna.ViewModel;
using TodaysManna.Views;
using static TodaysManna.Models.MccheyneRangeData;

namespace TodaysManna
{
    public partial class App : Xamarin.Forms.Application
    {
        public static ErrorPopup errorPopup;

        private static MemoItemDatabase database;

        public static MccheyneCheckViewModel mccheyneCheckViewModel;
        public static MccheyneCheckListPage mccheyneCheckListPage;
        public static List<MccheyneRange> mccheyneRanges;

        public static int OpenCount = 0;

        public App()
        {
            InitializeComponent();            
            CreateData();

            MainPage = new MainTabbedPage();
        }

        private void CreateData()
        {
            errorPopup = new ErrorPopup();

            mccheyneCheckViewModel = new MccheyneCheckViewModel();
            mccheyneCheckListPage = new MccheyneCheckListPage();

            mccheyneRanges = new List<MccheyneRange>();
            try
            {
                mccheyneRanges = GetJsonMccheyneRange();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail("# App GetJsonMccheyneRange() \n" + e.Message);
                ShowErrorPopup("맥체인 불러오기 오류");
            }
        }

        private List<MccheyneRange> GetJsonMccheyneRange()
        {
            var jsonFileName = "MccheyneRange.json";
            var ObjContactList = new MccheyneRangeList();
            var assembly = typeof(MannaPage).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Datas.{jsonFileName}");

            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                ObjContactList = JsonConvert.DeserializeObject<MccheyneRangeList>(jsonString);
            }

            return ObjContactList.Ranges;
        }

        public static MemoItemDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new MemoItemDatabase();
                }
                return database;
            }
        }

        public static async void ShowErrorPopup(string message)
        {
            errorPopup.errorLabel.Text = message;
            try
            {
                await PopupNavigation.Instance.PushAsync(errorPopup);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.Fail("ShowErrorPopup Error: " + e.Message);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
