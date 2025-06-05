using HomeIQ.ViewModels;
using Microsoft.Maui.Controls;

namespace HomeIQ.Views
{
    public partial class SecondPageView : ContentPage
    {
        public SecondPageView(SecondPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ViewModels.SecondPageViewModel vm)
                vm.LoadProgramsCommand.Execute(null);
        }
    }
}