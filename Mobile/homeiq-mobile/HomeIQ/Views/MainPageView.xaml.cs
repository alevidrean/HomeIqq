using HomeIQ.ViewModels;
using Microsoft.Maui.Controls;

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
            if (BindingContext is MainPageViewModel vm)
                await vm.RefreshTemperaturesAsync();
        }
    }
}