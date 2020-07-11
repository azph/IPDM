using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ONVIF_Manager.Models;

namespace ONVIF_Manager.Services
{
    public class MockDataStore : IDataStore<ConnectionInfo>
    {
        readonly List<ConnectionInfo> items;

        public MockDataStore()
        {
            items = new List<ConnectionInfo>()
            {
                new ConnectionInfo { Id = 1, Host = "60.191.94.122:9507", Username = "admin", Password = "admin123" },
                new ConnectionInfo { Id = 2, Host = "172.17.12.50:80", Username = "root", Password = "root" },
                new ConnectionInfo { Id = 3, Host = "172.17.12.89:80", Username = "admin", Password = "Hik12345+" },
                new ConnectionInfo { Id = 4, Host = "172.17.12.77:80", Username = "service", Password = "?g%WceYtcp1" },
                new ConnectionInfo { Id = 5, Host = "172.17.12.58:80", Username = "admin", Password = "X?kAmvN1FAGN" },
                new ConnectionInfo { Id = 6, Host = "172.17.12.52:80", Username = "admin", Password = "0eydozFnrrsF" },
                new ConnectionInfo { Id = 7, Host = "172.17.12.51:80", Username = "root", Password = "root1qaz" },
                new ConnectionInfo { Id = 8, Host = "172.17.12.108:80", Username = "admin", Password = "0eydozFnrrsF" },
                new ConnectionInfo { Id = 9, Host = "220.76.91.54:60001", Username = "admin", Password = "1qaz@WSX#EDC" }
            };
        }

        public async Task<bool> AddItemAsync(ConnectionInfo item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ConnectionInfo item)
        {
            var oldItem = items.Where((ConnectionInfo arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((ConnectionInfo arg) => arg.Id.ToString() == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ConnectionInfo> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id.ToString() == id));
        }

        public async Task<IEnumerable<ConnectionInfo>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}