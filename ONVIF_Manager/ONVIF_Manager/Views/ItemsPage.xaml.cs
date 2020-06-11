using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ONVIF_Manager.Models;
using ONVIF_Manager.Views;
using ONVIF_Manager.ViewModels;
using System.Windows.Input;
using Expandable;

namespace ONVIF_Manager.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ConnectionInfo;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(new BusinessLayer.DeviceImpl(item))));
            //await Navigation.PushAsync(new DeviceInfoPage(new DeviceInfoViewModel(new BusinessLayer.DeviceImpl(item))));
            //await Navigation.PushAsync(new NetworkSettingsPage(new NetworkSettingsViewModel(new BusinessLayer.DeviceImpl(item))));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //ItemsListView.SelectedItem. StatusChanged += OnStatusChanged;

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }


        private ICommand _tapCommand;
        public ICommand TapCommand => _tapCommand ?? (_tapCommand = new Command(p =>
        {
            //DisplayAlert("Tapped", p.ToString(), "Ok");
        }));

        private async void OnStatusChanged(object sender, StatusChangedEventArgs e)
        {

            var rotation = 0;
            switch (e.Status)
            {
                case ExpandStatus.Collapsing:
                    break;
                case ExpandStatus.Expanding:
                    rotation = 180;
                    break;
                default:
                    return;
            }

            await (sender as Expandable.ExpandableView).TouchHandlerView.RotateTo(rotation, 200, Easing.CubicInOut);
        }


    }
}