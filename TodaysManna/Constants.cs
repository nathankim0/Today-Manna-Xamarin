﻿using Xamarin.Forms;
using System;
using System.IO;
using Xamarin.Essentials;

namespace TodaysManna.Constants
{
    public static class Values
    {
        public static readonly bool IsDeviceIOS = Device.RuntimePlatform == Device.iOS;

        public static int MannaPageLaunchCount = 0;
        public static int MccheynePageLaunchCount = 0;
        public static int CheckListPageLaunchCount = 0;
        public static int MemoPageLaunchCount = 0;
        public static int SettingsPageLaunchCount = 0;

        public static readonly double height = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        public static readonly double width = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        public static readonly double StatusBarHeight = DependencyService.Get<IStatusBar>().GetHeight();
        public static double TabHeight { get; set; } = 0;
        public static double BottomSafeAreaHeight { get; set; } = 0;
    }

    public static class Colors
    {
        public static Color MccheyneColor1 = Color.FromHex("#c4a7fa");
        public static Color MccheyneColor2 = Color.FromHex("#9CC0BA");
        public static Color MccheyneColor3 = Color.FromHex("#D07D7B");
        public static Color MccheyneColor4 = Color.FromHex("#E7CBB0");
        public static Color MccheyneColor5 = Color.FromHex("#edafb3");
    }

    public static class Rests
    {
        public const string MannaEndpoint = "http://3.138.184.130:9179/api/v1/today-manna/";
        public const string DatabaseFilename = "MemoSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
    public static class MessagingCenterMessage
    {
        public static string ScrollMannaToTop = "ScrollMannaToTop";
        public static string ScrollMccheyneToTop = "ScrollMccheyneToTop";
        public static string ScrollCheckListToTop = "ScrollCheckListToTop";
        public static string ScrollMemoToTop = "ScrollMemoToTop";

        public static string ScrollHomePageToTop = "ScrollHomePageToTop";
    }

    public static class TitleNames
    {
        public static string Manna = "만나";
        public static string Mccheyne = "맥체인";
        public static string CheckList = "체크리스트";
        public static string Memo = "메모";
        public static string Settings = "설정";
    }
}
