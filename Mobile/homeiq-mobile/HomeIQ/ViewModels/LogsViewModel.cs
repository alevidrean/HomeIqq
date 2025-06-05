using HomeIQ.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HomeIQ.ViewModels
{
    public class LogsViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();

        public ObservableCollection<EventLog> Logs { get; } = new();

        public LogsViewModel()
        {
            LoadLogsCommand = new Command(async () => await LoadLogsAsync());
            // Load logs at startup
            LoadLogsCommand.Execute(null);
        }

        public ICommand LoadLogsCommand { get; }

        private async Task LoadLogsAsync()
        {
            Logs.Clear();
            var logs = await _apiService.GetLastEventLogsAsync();
            foreach (var log in logs)
                Logs.Add(log);
          
            OnPropertyChanged(nameof(AllLogsDetails));
            
        }

        public string AllLogsDetails
        {
            get
            {
                if (Logs == null || Logs.Count == 0)
                    return "No logs available.";

                return string.Join(
                    "\n\n",
                    Logs.Select(log =>
                        $"{log.EventType} | {log.Details} | {log.Timestamp:yyyy-MM-dd HH:mm:ss}"
                    )
                );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ICommand NavigateCommand => new Command<string>(async (route) =>
        {
            if (!string.IsNullOrEmpty(route))
                await Shell.Current.GoToAsync($"//{route}", true);
        });
    }
}