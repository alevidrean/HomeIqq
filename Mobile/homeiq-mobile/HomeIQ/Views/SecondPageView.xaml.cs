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
    }
}