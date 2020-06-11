using LibVLCSharp.Shared;
using ONVIF_Manager.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONVIF_Manager.ViewModels
{
    public class VideoStreamViewModel : BaseViewModel
    {
        public DeviceImpl Device { get; set; }

        private Onvif.DeviceClient _deviceClient;


        private MediaService.MediaClient _mediaClient;
        public VideoStreamViewModel(DeviceImpl device)
        {
            Device = device;


            Core.Initialize();
            LibVLC = new LibVLC();
        }
        private async Task<MediaService.MediaUri> loadAsync()
        {
            try
            {

                _deviceClient = Device.Client;

                var result = await Task.Run(() => _deviceClient.GetServices(IncludeCapability: false));

                var host = new Uri(Device.ConnectionInfo.Host).Host;

                _mediaClient = ServicesHelper.CreateServiceClient(
                    ServicesHelper.ReplaceHost(result.FirstOrDefault(service => service.Namespace == "http://www.onvif.org/ver10/media/wsdl").XAddr, host),
                    Device.ConnectionInfo.Username, Device.ConnectionInfo.Password, (binding, address) => new MediaService.MediaClient(binding, address)) as MediaService.MediaClient;


                var profiles = await Task.Run(() => _mediaClient.GetProfiles());

                var streamSetup = new MediaService.StreamSetup
                {
                    Stream = MediaService.StreamType.RTPUnicast,
                    Transport = new MediaService.Transport
                    {
                        Protocol = MediaService.TransportProtocol.RTSP
                    }
                };

                return await Task.Run(() => _mediaClient.GetStreamUri(streamSetup, profiles.First().token));

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                
            }


        }

        public async void OnAppearing()
        {
            var streamUri = await Task.Run(async () => await loadAsync());

            var host = new Uri(Device.ConnectionInfo.Host).Host;

            var media = new Media(LibVLC,
                new Uri(ServicesHelper.ReplaceHost(streamUri.Uri, host)));

            media.AddOption($":rtsp-user={Device?.ConnectionInfo.Username}");
            media.AddOption($":rtsp-pwd={Device?.ConnectionInfo.Password}");

            // Transport.
            media.AddOption($":rtsp-tcp");

            MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };

            if (!MediaPlayer.Play())
            {
                return;
            }

        }

        private MediaPlayer _mediaPlayer;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.MediaPlayer"/> instance.
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => SetProperty(ref _mediaPlayer, value, nameof(MediaPlayer));
        }

        private LibVLC _libVLC;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.LibVLC"/> instance.
        /// </summary>
        public LibVLC LibVLC
        {
            get => _libVLC;
            private set => SetProperty(ref _libVLC, value, nameof(LibVLC));
        }

    }
}
