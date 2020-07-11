using ONVIF_Manager.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;

namespace ONVIF_Manager.Models
{
    public class ConnectionInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        private DeviceImpl _deviceImpl;

        [Ignore]
        public string Uri => $"http://{Host}/onvif/device_service";

        [Ignore]
        public DeviceImpl DeviceImpl
        {
            get
            {
                if (_deviceImpl == null)
                {
                    _deviceImpl = new BusinessLayer.DeviceImpl(this);
                }
                return _deviceImpl;
            }
        }

        ObservableCollection<MediaService.VideoSource> _videoSources;

        [Ignore]
        public ObservableCollection<MediaService.VideoSource> VideoSources 
        { 
            get
            {
                if (_videoSources == null)
                {
                    _videoSources = new ObservableCollection<MediaService.VideoSource>();
                }

                return _videoSources;
            }
            set 
            {
                _videoSources = value;
            }
        }

    }
}