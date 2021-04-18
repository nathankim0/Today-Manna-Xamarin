using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using static TodaysManna.Models.JsonMccheyneRangeModel;

namespace TodaysManna
{
    public partial class App : Xamarin.Forms.Application
    {
        public static ErrorPopup errorPopup;

        private static MemoItemDatabaseService database;
        public static List<MccheyneRange> mccheyneRanges;

        public static int OpenCount = 0;

        public App()
        {
            InitializeComponent();

            CreateData();

            MainPage = new MainTabbedPage();
        }

        private async void CreateData()
        {
            mccheyneRanges = new List<MccheyneRange>();
            try
            {
                mccheyneRanges = GetJsonMccheyneRange();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Fail("# App GetJsonMccheyneRange() \n" + e.Message);
                await MainPage.DisplayAlert("맥체인 불러오기 오류", "", "확인");
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

        public static MemoItemDatabaseService Database
        {
            get
            {
                if (database == null)
                {
                    database = new MemoItemDatabaseService();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("start_app");
        }

        protected override void OnSleep()
        {
            FirebaseEventService.SendEventOnPlatformSpecific("sleep_app");
        }

        protected override void OnResume()
        {
        }
    }
}
