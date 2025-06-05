using HomeIQ.ViewModels;
using Microsoft.Maui.Controls;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace HomeIQ.Views
{
    public partial class MainPageView : ContentPage
    {
        public MainPageView(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //var ble = CrossBluetoothLE.Current;
            //if (ble.State != BluetoothState.On)
            //   {
            //    await DisplayAlert("Bluetooth", "Te rugăm să pornești Bluetooth-ul pentru a folosi aplicația.", "OK");
            //}
            if (BindingContext is MainPageViewModel vm)
                await vm.RefreshTemperaturesAsync();
        }
    }
}