using ONVIF_Manager.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ONVIF_Manager.ViewModels
{
    public class VideoSourcesViewModel : BaseViewModel
    {
        public DeviceImpl DeviceImpl { get; set; }

        public VideoSourcesViewModel()
        {
            
        }
    }
}
