using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using ONVIF_Manager.Models;
using LibVLCSharp.Shared;
using System.Linq;
using System.Threading;
using ONVIF_Manager.BusinessLayer;
using Xamarin.Forms;
using ONVIF_Manager.Views;
using System.Windows.Input;

namespace ONVIF_Manager.ViewModels
{

    public class ItemDetailViewModel : BaseViewModel
    {

        public DeviceImpl DeviceImpl { get; set; }

        public ItemDetailViewModel(DeviceImpl device)
        {
            Title = device?.ConnectionInfo.Host;
            DeviceImpl = device;


            this.SettingsButtonClicked = new Command(async () =>
            {
                await Task.Run(() => { MediaPlayer.Stop(); });
            });

            this.MuteButtonClicked = new Command( () =>
            {
                _mediaPlayer.Mute = !_mediaPlayer.Mute;
            });

            this.TakeSnapshotButtonClicked = new Command(() =>
            {
                _mediaPlayer.TakeSnapshot(0, $"/sdcard/Images/test_image.jpg", 0, 0);
            });

            Core.Initialize();
            LibVLC = new LibVLC();
        }


        #region Commands
        public ICommand SettingsButtonClicked
        {
             get; private set;
        }

        public ICommand MuteButtonClicked
        {
            get; private set;
        }
        public ICommand TakeSnapshotButtonClicked
        {
            get; private set;
        }

        #endregion

        private void onSnapshotTaken(object sender, MediaPlayerSnapshotTakenEventArgs e)
        {
            // Send to view.
            //await DisplayAlert("Snapshot saved", "An item was tapped.", "OK");
        }

        private async  Task<string> loadAsync()
        {
            try
            {
                
                var result = await DeviceImpl.getServicesAsync(IncludeCapability: false);

                var host = new Uri(DeviceImpl.ConnectionInfo.Host).Host;

                if (DeviceImpl.Media2Service != null)
                {
                    var media2Client = ServicesHelper.CreateServiceClient(
                        ServicesHelper.ReplaceHost(DeviceImpl.Media2ServiceAddr, host),
                        DeviceImpl.ConnectionInfo.Username, DeviceImpl.ConnectionInfo.Password, (binding, address) => new Media2Service.Media2Client(binding, address)) as Media2Service.Media2Client;


                    var profiles2 = await Task.Run(() => media2Client.GetProfiles(null, new string[] { "All" }));

                    return await Task.Run(() => media2Client.GetStreamUri("RTSP", profiles2.First().token));
                }

                var mediaClient = ServicesHelper.CreateServiceClient(
                    ServicesHelper.ReplaceHost(DeviceImpl.MediaServiceAddr, host),
                    DeviceImpl.ConnectionInfo.Username, DeviceImpl.ConnectionInfo.Password, (binding, address) => new MediaService.MediaClient(binding, address)) as MediaService.MediaClient;

                var profiles = await Task.Run(() => mediaClient.GetProfiles());

                var streamSetup = new MediaService.StreamSetup
                {
                    Stream = MediaService.StreamType.RTPUnicast,
                    Transport = new MediaService.Transport
                    {
                        Protocol = MediaService.TransportProtocol.RTSP
                    }
                };

                return  await Task.Run(() => mediaClient.GetStreamUri(streamSetup, profiles.First().token).Uri);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Console.WriteLine($" End loadAsync call {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        private LibVLC _libVLC;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.LibVLC"/> instance.
        /// </summary>
        public LibVLC LibVLC
        {
            get => _libVLC;
            private set => Set(nameof(LibVLC), ref _libVLC, value);
        }

        private MediaPlayer _mediaPlayer;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.MediaPlayer"/> instance.
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(nameof(MediaPlayer), ref _mediaPlayer, value);
        }
        /// <summary>
        /// Initialize LibVLC and playback when page appears
        /// </summary>
        public async void OnAppearing()
        {
            var streamUri = await Task.Run(async  ()  => await  loadAsync());

            var host = new Uri(DeviceImpl.ConnectionInfo.Host).Host;

            var media = new Media(LibVLC,
                new Uri(ServicesHelper.ReplaceHost(streamUri, host)));

            media.AddOption($":rtsp-user={DeviceImpl?.ConnectionInfo.Username}");
            media.AddOption($":rtsp-pwd={DeviceImpl?.ConnectionInfo.Password}");
            
            // Transport.
            media.AddOption($":rtsp-tcp");

            MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true };

            MediaPlayer.SnapshotTaken += onSnapshotTaken;

            if (!MediaPlayer.Play())
            {
                return;
            }

        }

        private void Set<T>(string propertyName, ref T field, T value)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;

                OnPropertyChanged(propertyName);
            }
        }


    }
}
