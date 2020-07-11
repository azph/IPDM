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
                        ConnectionInfo?.Uri, ConnectionInfo.Username, ConnectionInfo.Password, (binding, address) => new Onvif.DeviceClient(binding, address)) as Onvif.DeviceClient;
                }

                return _client;
            }
        }

        MediaService.MediaClient _mediaClient;
        public MediaService.MediaClient MediaClient
        {
            get
            {
                if (_mediaClient == null)
                {
                    var host = new Uri(ConnectionInfo.Uri).Host;
                    _mediaClient = ServicesHelper.CreateServiceClient(
                    ServicesHelper.ReplaceHost(MediaServiceAddr, host),
                    ConnectionInfo.Username, ConnectionInfo.Password, (binding, address) => new MediaService.MediaClient(binding, address)) as MediaService.MediaClient;
                }

                return _mediaClient;
            }
        }

        Media2Service.Media2Client _media2Client;
        public Media2Service.Media2Client Media2Client
        {
            get
            {
                if (_media2Client == null)
                {
                    var host = new Uri(ConnectionInfo.Uri).Host;
                    _media2Client = ServicesHelper.CreateServiceClient(
                    ServicesHelper.ReplaceHost(Media2ServiceAddr, host),
                    ConnectionInfo.Username, ConnectionInfo.Password, (binding, address) => new Media2Service.Media2Client(binding, address)) as Media2Service.Media2Client;
                }

                return _media2Client;
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
                var host = new Uri(ConnectionInfo.Uri).Host;
                return ServicesHelper.ReplaceHost(MediaService?.XAddr, host);
            }
        }

        public string Media2ServiceAddr
        {
            get
            {
                var host = new Uri(ConnectionInfo.Uri).Host;
                return ServicesHelper.ReplaceHost(Media2Service?.XAddr, host);
            }
        }
    }
}
