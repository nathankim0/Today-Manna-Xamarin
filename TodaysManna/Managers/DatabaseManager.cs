namespace TodaysManna.Managers
{
    public static class DatabaseManager
    {
        private static MemoItemDatabaseService _database;
        public static MemoItemDatabaseService Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new MemoItemDatabaseService();
                }
                return _database;
            }
        }
    }
}
