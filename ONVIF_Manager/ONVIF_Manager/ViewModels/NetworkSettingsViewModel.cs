using ONVIF_Manager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ONVIF_Manager.ViewModels
{
    public class NetworkSettingsViewModel : BaseViewModel
    {
        private BusinessLayer.DeviceImpl _deviceIpml;
        private string _ipAddress;
        public string IPAddress
        {
            get { return _ipAddress; }
            set { SetProperty(ref _ipAddress, value); }
        }

        private string _mask;
        public string Mask
        {
            get { return _mask; }
            set { SetProperty(ref _mask, value); }
        }

        private string _gateway;
        public string Gateway
        {
            get { return _gateway; }
            set { SetProperty(ref _gateway, value); }
        }

        private bool _dhcp;
        public bool DHCP
        {
            get { return _dhcp; }
            set { SetProperty(ref _dhcp, value); }
        }

        private int _httpPort;
        public int HTTPPort
        {
            get { return _httpPort; }
            set { SetProperty(ref _httpPort, value); }
        }

        private bool _httpState;
        public bool HTTPState
        {
            get { return _httpState; }
            set { SetProperty(ref _httpState, value); }
        }

        private int _httpsPort;
        public int HTTPSPort
        {
            get { return _httpsPort; }
            set { SetProperty(ref _httpsPort, value); }
        }

        private bool _httpsState;
        public bool HTTPSState
        {
            get { return _httpsState; }
            set { SetProperty(ref _httpsState, value); }
        }

        private int _rtspPort;
        public int RTSPPort
        {
            get { return _rtspPort; }
            set { SetProperty(ref _rtspPort, value); }
        }

        private bool _rtspState;
        public bool RTSPState
        {
            get { return _rtspState; }
            set { SetProperty(ref _rtspState, value); }
        }



        public NetworkSettingsViewModel(BusinessLayer.DeviceImpl deviceIpml)
        {
            _deviceIpml = deviceIpml;

        }

        public async void OnAppearing()
        {
            IsBusy = true;
            await loadData();

            IsBusy = false;
        }

        private async Task loadData()
        {
            var networkInterfaces = await Task.Run(() => _deviceIpml.Client.GetNetworkInterfaces());

            var config = networkInterfaces.First().IPv4.Config;

            DHCP = config.DHCP;
            Onvif.PrefixedIPv4Address prefAddr;
            if (DHCP)
            {
                prefAddr = config.FromDHCP;
            }
            else
            {
                prefAddr = config.Manual.First();
            }

            IPAddress = prefAddr.Address;

            Mask = BusinessLayer.SubnetMask.CreateByHostBitLength(prefAddr.PrefixLength).ToString();

            var networkGateway = await Task.Run(() => _deviceIpml.Client.GetNetworkDefaultGateway());

            Gateway = networkGateway.IPv4Address.First();

            var networkProtocols = await Task.Run(() => _deviceIpml.Client.GetNetworkProtocols());

            var http = networkProtocols.FirstOrDefault(pr => pr.Name == Onvif.NetworkProtocolType.HTTP);
            if (http != null)
            {
                HTTPPort = http.Port.First();
                HTTPState = http.Enabled;
            }

            var https = networkProtocols.FirstOrDefault(pr => pr.Name == Onvif.NetworkProtocolType.HTTPS);
            if (https != null)
            {
                HTTPSPort = https.Port.First();
                HTTPSState = https.Enabled;
            }

            var rtsp = networkProtocols.FirstOrDefault(pr => pr.Name == Onvif.NetworkProtocolType.RTSP);
            if (rtsp != null)
            {
                RTSPPort = rtsp.Port.First();
                RTSPState = rtsp.Enabled;
            }

        }

    }
}
