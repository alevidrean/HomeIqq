using Microsoft.Maui.Controls;
using HomeIQ.ViewModels;

namespace HomeIQ.Views
{
    public partial class LoginPageView : ContentPage
    {
        public LoginPageView(LoginPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
