using HomeIQ.ViewModels;

namespace HomeIQ.Views;

public partial class LogsView : ContentPage
{
    public LogsView(LogsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // or use DI for the ViewModel only
    }
}