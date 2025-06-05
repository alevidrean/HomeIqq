using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HomeIQ.Services;
using Microsoft.Maui.Controls;
using System.Linq;

namespace HomeIQ.ViewModels
{
    public class SecondPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ApiService _apiService = new ApiService();

        public ObservableCollection<TemperatureProgramDto> Programs { get; } = new();

        private TemperatureProgramDto _selectedProgram;
        public TemperatureProgramDto SelectedProgram
        {
            get => _selectedProgram;
            set
            {
                if (_selectedProgram != value)
                {
                    _selectedProgram = value;
                    OnPropertyChanged();
                }
            }
        }

        private TemperatureProgramDto _popupProgram;
        public TemperatureProgramDto PopupProgram
        {
            get => _popupProgram;
            set
            {
                if (_popupProgram != value)
                {
                    _popupProgram = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsPopupVisible));
                }
            }
        }

        public bool IsPopupVisible => PopupProgram != null;

        public SecondPageViewModel()
        {
            LoadProgramsCommand = new Command(async () => await LoadProgramsAsync());
            ShowProgramPopupCommand = new Command<TemperatureProgramDto>(program => PopupProgram = program);
            HidePopupCommand = new Command(() => PopupProgram = null);
            SelectPopupProgramCommand = new Command(async () => await SelectPopupProgramAsync());

            MessagingCenter.Subscribe<MainPageViewModel>(this, "TemperatureSet", (sender) =>
            {
                SelectedProgram = null;
            });

            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
            {
                // Only refresh if the popup is not open
                if (!IsPopupVisible)
                    MainThread.BeginInvokeOnMainThread(async () => await LoadProgramsAsync());
                return true; // Return true to keep the timer running
            });
        }

        public ICommand LoadProgramsCommand { get; }
        public ICommand ShowProgramPopupCommand { get; }
        public ICommand HidePopupCommand { get; }
        public ICommand SelectPopupProgramCommand { get; }

        private async Task LoadProgramsAsync()
        {
            var previousPopupId = PopupProgram?.Id;
            Programs.Clear();
            var programs = await _apiService.GetProgramsAsync();
            foreach (var p in programs)
                Programs.Add(p);

            SelectedProgram = Programs.FirstOrDefault(p => p.IsActive);

            // Re-assign PopupProgram to the refreshed instance if it still exists
            if (previousPopupId != null)
                PopupProgram = Programs.FirstOrDefault(p => p.Id == previousPopupId);
        }

        private async Task SelectPopupProgramAsync()
        {
            if (PopupProgram != null && !PopupProgram.IsActive)
            {
                await _apiService.ActivateProgramAsync(PopupProgram.Name);
                await LoadProgramsAsync();
            }
            PopupProgram = null;
        }

        public ICommand SelectProgramDirectCommand => new Command<TemperatureProgramDto>(async (program) =>
        {
            if (program != null && !program.IsActive)
            {
                await _apiService.ActivateProgramAsync(program.Name);
                await LoadProgramsAsync();
            }
        });

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ICommand NavigateCommand => new Command<string>(async (route) =>
        {
            if (!string.IsNullOrEmpty(route))
                await Shell.Current.GoToAsync($"//{route}", true);
        });
    }
}