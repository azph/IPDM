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
    public partial class NetworkSettingsPage : ContentPage
    {
        NetworkSettingsViewModel _viewModel;
        public NetworkSettingsPage(NetworkSettingsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this._viewModel = viewModel;
        }

        void OnAppearing(object sender, System.EventArgs e)
        {
            base.OnAppearing();

            (BindingContext as NetworkSettingsViewModel).OnAppearing();

        }
    }
}