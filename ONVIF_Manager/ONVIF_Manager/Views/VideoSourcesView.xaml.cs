using ONVIF_Manager.BusinessLayer;
using ONVIF_Manager.Models;
using ONVIF_Manager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ONVIF_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoSourcesView : ContentView
    {
        public ObservableCollection<MediaService.VideoSource> VideoSources { get; set; }
        public DeviceImpl DeviceImpl
        {
            get { return (DeviceImpl)GetValue(DeviceImplProperty); }
            set { SetValue(DeviceImplProperty, value); }
        }

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static void DeviceImplChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }
        public static readonly BindableProperty DeviceImplProperty =
            BindableProperty.Create("DeviceImpl", typeof(DeviceImpl), typeof(VideoSourcesView), default(DeviceImpl), BindingMode.TwoWay, null, DeviceImplChanged);
        public static readonly BindableProperty IsBusyProperty = 
            BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(VideoSourcesView), default(bool), defaultBindingMode: BindingMode.TwoWay);


        public VideoSourcesView()
        {
            InitializeComponent();


            VideoSources = new ObservableCollection<MediaService.VideoSource>();
            
            this.LayoutChanged += (s, e) =>
            {

               Device.BeginInvokeOnMainThread(async () =>
                {
                    var info = BindingContext as ConnectionInfo;

                    if (info.VideoSources.Any() || IsBusy)
                    {
                        return;
                    }
                    IsBusy = true;
                    await DeviceImpl.getServicesAsync(false);

                    var videoSources = await Task.Run(() => DeviceImpl.MediaClient.GetVideoSources());
                   
                    foreach (var vs in videoSources)
                    {
                        
                        
                        info.VideoSources.Add(vs);
                    }
                    IsBusy = false;
                });
            };
            
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}