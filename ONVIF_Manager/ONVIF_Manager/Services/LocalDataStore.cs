using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ONVIF_Manager.Models;
using SQLite;

namespace ONVIF_Manager.Services
{
    public class LocalDataStore : IDataStore<ConnectionInfo>
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public LocalDataStore()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ConnectionInfo).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ConnectionInfo)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }

        public async Task<bool> AddItemAsync(ConnectionInfo item)
        {
            return Convert.ToBoolean(await Database.InsertAsync(item));
        }

        public async Task<bool> UpdateItemAsync(ConnectionInfo item)
        {
            return Convert.ToBoolean(await Database.UpdateAsync(item));
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            return Convert.ToBoolean(await Database.DeleteAsync(Convert.ToInt32(id)));
        }

        public async Task<ConnectionInfo> GetItemAsync(string id)
        {
            return await Database.Table<ConnectionInfo>().Where(i => i.Id.ToString() == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ConnectionInfo>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Database.Table<ConnectionInfo>().ToListAsync();
        }
    }
}