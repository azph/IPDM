using ONVIF_Manager.BusinessLayer;
using System;
using System.Collections.Generic;

namespace ONVIF_Manager.Models
{
    public class ConnectionInfo
    {
        public string Id { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        private DeviceImpl _deviceImpl;
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
        

    }
}