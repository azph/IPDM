using ONVIF_Manager.ViewModels;
using System;
using System.Collections.Generic;
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
        public VideoSourcesView()
        {
            InitializeComponent();
            this.LayoutChanged += (s, e) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    
                });
            };
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}