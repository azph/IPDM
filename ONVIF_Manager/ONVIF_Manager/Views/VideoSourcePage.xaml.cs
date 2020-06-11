using ONVIF_Manager.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ONVIF_Manager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VideoSourcePage : ContentPage
    {

        VideoSourceViewModel _viewModel;

        public VideoSourcePage(VideoSourceViewModel viewModel)
        {
            InitializeComponent();



            BindingContext = _viewModel = viewModel;
           
        }

        void OnAppearing(object sender, System.EventArgs e)
        {
            base.OnAppearing();

            (BindingContext as VideoSourceViewModel).OnAppearing();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
