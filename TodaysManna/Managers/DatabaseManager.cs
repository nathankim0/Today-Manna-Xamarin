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

        private static MccheyneCheckListDatabaseService _checkListDatabase;
        public static MccheyneCheckListDatabaseService CheckListDatabase
        {
            get
            {
                if (_checkListDatabase == null)
                {
                    _checkListDatabase = new MccheyneCheckListDatabaseService();
                }
                return _checkListDatabase;
            }
        }
    }
}
