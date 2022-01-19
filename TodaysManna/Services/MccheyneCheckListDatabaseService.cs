using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;

using TodaysManna.Models;

namespace TodaysManna
{
    public class MccheyneCheckListDatabaseService
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Rests.DatabasePath, Rests.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public MccheyneCheckListDatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(MccheyneCheckListLocalContent).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(MccheyneCheckListLocalContent)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public SQLiteAsyncConnection GetDB()
        {
            return Database;//. Table<MccheyneCheckListLocalContent>();
        }

        public Task<List<MccheyneCheckListLocalContent>> GetItemsAsync()
        {
            return Database.Table<MccheyneCheckListLocalContent>().ToListAsync();
        }

        //public Task<List<MccheyneCheckListLocalContent>> GetItemsNotDoneAsync()
        //{
        //    //return Database.QueryAsync<MccheyneCheckListLocalContent>("SELECT * FROM [MccheyneCheckListLocalContent] WHERE [Done] = 0");
        //}

        public Task<MccheyneCheckListLocalContent> GetItemAsync(int id)
        {
            return Database.Table<MccheyneCheckListLocalContent>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(MccheyneCheckListLocalContent item)
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

        public Task<int> DeleteItemAsync(MccheyneCheckListLocalContent item)
        {
            return Database.DeleteAsync(item);
        }
    }
}