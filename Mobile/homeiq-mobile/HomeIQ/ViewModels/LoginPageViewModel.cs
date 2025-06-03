using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeIQ.Views;
using HomeIQ.Services; // Add this for ApiService

namespace HomeIQ.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public string username;

        [ObservableProperty]
        public string password;

        [ObservableProperty]
        public string errorMessage;

        [ObservableProperty]
        public bool hasError;

        public ICommand LoginCommand { get; }

        private readonly IServiceProvider _serviceProvider;
        private readonly ApiService _apiService;

        public LoginPageViewModel(IServiceProvider serviceProvider, ApiService apiService)
        {
            _serviceProvider = serviceProvider;
            _apiService = apiService;
            LoginCommand = new AsyncRelayCommand(OnLoginAsync);
        }

        private async Task OnLoginAsync()
        {
            HasError = false;
            ErrorMessage = string.Empty;

            try
            {
                var result = await _apiService.LoginAsync(Username, Password);
                if (result != null)
                {
                    Debug.WriteLine("Login successful");
                    await Shell.Current.GoToAsync($"//MainPageView?Username={Username}");
                }
                else
                {
                    Debug.WriteLine("Login failed");
                    ErrorMessage = "Invalid username or password.";
                    HasError = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login error: {ex.Message}");
                ErrorMessage = "Cannot connect to server. Please try again later.";
                HasError = true;
            }
        }
    }
}