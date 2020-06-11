using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ONVIF_Manager.Models;
using ONVIF_Manager.ViewModels;
using LibVLCSharp.Shared;

namespace ONVIF_Manager.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }


        void OnAppearing(object sender, System.EventArgs e)
        {
            base.OnAppearing();
            var model = BindingContext as ItemDetailViewModel;

            model.OnAppearing();
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VideoSourcePage(new VideoSourceViewModel(viewModel.DeviceImpl)));
        }
    }
}