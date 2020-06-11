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
    public partial class VideoStreamView : ContentView
    {
        VideoStreamViewModel viewModel;
        public VideoStreamView(VideoStreamViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        public void OnAppearing(object sender, System.EventArgs e)
        {
            var model = BindingContext as VideoStreamViewModel;

            model.OnAppearing();
        }
    }
}