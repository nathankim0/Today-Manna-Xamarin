using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using TodaysManna.Constants;
using TodaysManna.Models;

namespace TodaysManna
{
    public class MccheyneCheckItemDatabaseService
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Rests.CheckListDatabasePath, Rests.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public MccheyneCheckItemDatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(MccheyneCheckItem).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(MccheyneCheckItem)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public SQLiteAsyncConnection GetDB()
        {
            return Database;//. Table<MccheyneCheckItem>();
        }

        public Task<List<MccheyneCheckItem>> GetItemsAsync()
        {
            return Database.Table<MccheyneCheckItem>().ToListAsync();
        }

        //public Task<List<MccheyneCheckItem>> GetItemsNotDoneAsync()
        //{
        //    //return Database.QueryAsync<MccheyneCheckItem>("SELECT * FROM [MccheyneCheckItem] WHERE [Done] = 0");
        //}

        public async Task<bool> GetItemIsChecked(string id)
        {
            var items = await Database.QueryAsync<MccheyneCheckItem>($"SELECT * FROM [MccheyneCheckItem] WHERE [CheckIndex] = {id}");
            if(items.Count == 0)
            {
                return false;
            }
            else
            {
                var item = items.FirstOrDefault();
                var ischecked = item.IsChecked;
                return ischecked;
            }
        }

        public Task<MccheyneCheckItem> GetItemAsync(int id)
        {
            return Database.Table<MccheyneCheckItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(MccheyneCheckItem item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(MccheyneCheckItem item)
        {
            return Database.DeleteAsync(item);
        }
    }
}