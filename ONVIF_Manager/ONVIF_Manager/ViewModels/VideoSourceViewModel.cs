using ONVIF_Manager.BusinessLayer;
using ONVIF_Manager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace ONVIF_Manager.ViewModels
{

    public class VideoSourceViewModel : BaseViewModel
    {
        public DeviceImpl DeviceImpl { get; set; }
        public VideoSourceViewModel(BusinessLayer.DeviceImpl deviceIpml)
        {
            DeviceImpl = deviceIpml;

            Items = new ObservableCollection<ONVIF_Manager.Models.MProfile>
            {
            };
        }
        public ObservableCollection<ONVIF_Manager.Models.MProfile> Items { get; set; }

        public Command LoadItemsCommand { get; set; }

        public async void OnAppearing()
        {
            IsBusy = true;
            await loadData();

            IsBusy = false;
        }

        private async Task loadData()
        {
            if (DeviceImpl.Media2Service != null)
            {
                var media2Client = ServicesHelper.CreateServiceClient(
                    DeviceImpl.Media2ServiceAddr, 
                    DeviceImpl.ConnectionInfo.Username, DeviceImpl.ConnectionInfo.Password, (binding, address) => new Media2Service.Media2Client(binding, address)) as Media2Service.Media2Client;


                var profiles2 = await Task.Run(() => media2Client.GetProfiles(null, new string[] { "All" }));

                profiles2.Select(o => new MProfile {Token = o.token, Name = o.Name }).ForEach(oo=> Items.Add(oo));
                return;
            }

            var mediaClient = ServicesHelper.CreateServiceClient(
                DeviceImpl.MediaServiceAddr,
                DeviceImpl.ConnectionInfo.Username, DeviceImpl.ConnectionInfo.Password, (binding, address) => new MediaService.MediaClient(binding, address)) as MediaService.MediaClient;

            var profiles = await Task.Run(() => mediaClient.GetProfiles());
            profiles.Select(o => new MProfile { Token = o.token, Name = o.Name }).ForEach(oo => Items.Add(oo));
        }
    }
}
