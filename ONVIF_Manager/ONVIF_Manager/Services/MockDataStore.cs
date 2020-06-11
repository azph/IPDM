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
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://60.191.94.122:9507/onvif/device_service", Username = "admin", Password = "admin123" },
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://172.17.12.50:80/onvif/device_service", Username = "root", Password = "root" },
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://172.17.12.89:80/onvif/device_service", Username = "admin", Password = "Hik12345+" },
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://172.17.12.77:80/onvif/device_service", Username = "service", Password = "?g%WceYtcp1" },
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://172.17.12.58:80/onvif/device_service", Username = "admin", Password = "X?kAmvN1FAGN" },
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://172.17.12.52:80/onvif/device_service", Username = "admin", Password = "0eydozFnrrsF" },
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://172.17.12.51:80/onvif/device_service", Username = "root", Password = "root1qaz" },
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://172.17.12.108:80/onvif/device_service", Username = "admin", Password = "0eydozFnrrsF" },
                new ConnectionInfo { Id = Guid.NewGuid().ToString(), Host = "http://220.76.91.54:60001/onvif/device_service", Username = "admin", Password = "1qaz@WSX#EDC" }
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
            var oldItem = items.Where((ConnectionInfo arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ConnectionInfo> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ConnectionInfo>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}