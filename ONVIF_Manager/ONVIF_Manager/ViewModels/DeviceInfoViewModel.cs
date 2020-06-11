using ONVIF_Manager.BusinessLayer;
using ONVIF_Manager.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ONVIF_Manager.ViewModels
{
    public class DeviceInfoViewModel : BaseViewModel
    {
        string _manufacturer;
        public string Manufacturer
        {
            get { return _manufacturer; }
            set { SetProperty(ref _manufacturer, value); }
        }

        string _model;
        public string Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }


        string _firmwareVersion;
        public string FirmwareVersion
        {
            get { return _firmwareVersion; }
            set {  SetProperty(ref _firmwareVersion, value); }
        }

        string _serialNumber;
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { SetProperty(ref _serialNumber, value); }
        }

        string _hardwareId;
        public string HardwareId
        {
            get { return _hardwareId; }
            set { SetProperty(ref _hardwareId, value); }
        }


        private BusinessLayer.DeviceImpl _deviceImpl;
        public DeviceInfoViewModel(DeviceImpl deviceImpl)
        {
            _deviceImpl = deviceImpl;
        }

        public async void OnAppearing()
        {

            await loadData();
        }

        async Task loadData()
        {

            await Task.Run(() =>
            {
                Manufacturer = _deviceImpl.Client.GetDeviceInformation(out string model, out string firmwareVersion, out string serialNumber, out string hardwareId);
                Model = model;
                FirmwareVersion = firmwareVersion;
                SerialNumber = serialNumber;
                HardwareId = hardwareId;
            });
        }
    }
}
