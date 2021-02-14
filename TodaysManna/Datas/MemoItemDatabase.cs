using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using TodaysManna.Models;

namespace TodaysManna.Datas
{
    public class MemoItemDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public MemoItemDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(MemoItem).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(MemoItem)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<MemoItem>> GetItemsAsync()
        {
            return Database.Table<MemoItem>().ToListAsync();
        }

        //public Task<List<MemoItem>> GetItemsNotDoneAsync()
        //{
        //    //return Database.QueryAsync<MemoItem>("SELECT * FROM [MemoItem] WHERE [Done] = 0");
        //}

        public Task<MemoItem> GetItemAsync(int id)
        {
            return Database.Table<MemoItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(MemoItem item)
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

        public Task<int> DeleteItemAsync(MemoItem item)
        {
            return Database.DeleteAsync(item);
        }

    }

    public static class TaskExtensions
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }

    }
}