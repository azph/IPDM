using ONVIF_Manager.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ONVIF_Manager.ViewModels
{
    public class VideoSourcesViewModel : BaseViewModel
    {

        public ICommand LoadVideoSourcesCommand
        {
            get; private set;
        }

        public DeviceImpl OnvifDeviceImpl { get; set; }

        public VideoSourcesViewModel()
        {

            this.LoadVideoSourcesCommand = new Command(async () =>
            {
                await OnvifDeviceImpl.getServicesAsync(false);

                var videoSources = await Task.Run(() => OnvifDeviceImpl.MediaClient.GetVideoSources());
                foreach( var vs in videoSources)
                {
                    //VideoSources.Add(vs);
                }
            });

        }
    }
}
