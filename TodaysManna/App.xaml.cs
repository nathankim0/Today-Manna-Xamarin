using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using TodaysManna.Services;
using TodaysManna.Models;

namespace TodaysManna
{
    public partial class App : Application
    {
        public static ErrorPopup errorPopup;
        private static MemoItemDatabaseService database;
        private static MccheyneCheckItemDatabaseService checklistDatabase;
        public static List<MccheyneRange> mccheyneRanges;

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
                mccheyneRanges = GetJsonService.GetMccheyneRangesFromJson();
            }
            catch (Exception e)
            {
                Debug.Fail("# App GetJsonMccheyneRange() \n" + e.Message);
                await MainPage.DisplayAlert("맥체인 불러오기 오류", "", "확인");
            }
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

        public static MccheyneCheckItemDatabaseService ChecklistDatabase
        {
            get
            {
                if (checklistDatabase == null)
                {
                    checklistDatabase = new MccheyneCheckItemDatabaseService();
                }
                return checklistDatabase;
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
