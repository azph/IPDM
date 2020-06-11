using Onvif;
using ONVIF_Manager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONVIF_Manager.BusinessLayer
{
    public class DeviceImpl
    {
        private Onvif.Service[] _services;
        public ConnectionInfo ConnectionInfo { get;  private set; }
        public DeviceImpl(ConnectionInfo info)
        {
            ConnectionInfo = info;
        }

        Onvif.DeviceClient _client;
        public Onvif.DeviceClient Client
        {
            get 
            {
                if (_client == null)
                {
                    _client = ServicesHelper.CreateServiceClient(
                        ConnectionInfo?.Host, ConnectionInfo.Username, ConnectionInfo.Password, (binding, address) => new Onvif.DeviceClient(binding, address)) as Onvif.DeviceClient;
                }

                return _client;
            }
        }
        public async Task<Onvif.Service[]> getServicesAsync(bool IncludeCapability)
        {
            if (_services == null)
            {
                _services = await Task.Run(() => Client.GetServices(IncludeCapability));
            }

            return _services; 

        }

        public Onvif.Service DeviceService
        {
            get
            {
                return _services.FirstOrDefault(service => service.Namespace == "http://www.onvif.org/ver10/device/wsdl");
            }
        }

        public Onvif.Service  MediaService
        {
            get 
            {
                return _services.FirstOrDefault(service => service.Namespace == "http://www.onvif.org/ver10/media/wsdl");
            }
        }

        public Onvif.Service Media2Service
        {
            get
            {
                return _services.FirstOrDefault(service => service.Namespace == "http://www.onvif.org/ver20/media/wsdl");
            }
        }

        public string MediaServiceAddr
        {
            get
            {
                var host = new Uri(ConnectionInfo.Host).Host;
                return ServicesHelper.ReplaceHost(MediaService?.XAddr, host);
            }
        }

        public string Media2ServiceAddr
        {
            get
            {
                var host = new Uri(ConnectionInfo.Host).Host;
                return ServicesHelper.ReplaceHost(Media2Service?.XAddr, host);
            }
        }
    }
}
