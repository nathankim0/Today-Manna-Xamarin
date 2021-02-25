using System;
using System.IO;

namespace TodaysManna
{
    public class Constants
    {
        public const string MannaEndpoint = "http://3.138.184.130:9179/api/v1/today-manna/";
        public const string DatabaseFilename = "MemoSQLite.db3";
        public const string OneSignalAppID = "adc1c000-02c1-4c08-8313-bbdadc331645";

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
}
