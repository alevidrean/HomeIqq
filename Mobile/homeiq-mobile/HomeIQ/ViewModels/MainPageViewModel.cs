using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HomeIQ.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.Diagnostics;


namespace HomeIQ.ViewModels
{

    [QueryProperty(nameof(Username), "Username")]
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly ApiService _apiService = new ApiService();

        public MainPageViewModel()
        {

            // Pornește timerul la fiecare secundă
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _ = RefreshTemperaturesAsync();
                CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                return true; // continuă timerul
            });
        }

        void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));      

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _currentTime;
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _livingRoomTemperatureText;
        public string LivingRoomTemperatureText
        {
            get => _livingRoomTemperatureText;
            set
            {
                if (_livingRoomTemperatureText != value)
                {
                    _livingRoomTemperatureText = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _bedroomTemperatureText;
        public string BedroomTemperatureText
        {
            get => _bedroomTemperatureText;
            set
            {
                if (_bedroomTemperatureText != value)
                {
                    _bedroomTemperatureText = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _livingRoomHumidityText;
        public string LivingRoomHumidityText
        {
            get => _livingRoomHumidityText;
            set { _livingRoomHumidityText = value; OnPropertyChanged(); }
        }

        private string _bedroomHumidityText;
        public string BedroomHumidityText
        {
            get => _bedroomHumidityText;
            set { _bedroomHumidityText = value; OnPropertyChanged(); }
        }

        // Exemplu: incarcare temperaturi din backend
        public async Task RefreshTemperaturesAsync()
        {
            var data = await _apiService.GetCurrentTemperatureAsync();
            LivingRoomTemperatureText = data?.Camera1?.Temperature != null
                ? $"{data.Camera1.Temperature:0.0}°C"
                : "-";
            BedroomTemperatureText = data?.Camera2?.Temperature != null
                ? $"{data.Camera2.Temperature:0.0}°C"
                : "-";
            LivingRoomHumidityText = data?.Camera1?.Humidity != null
            ? $"{data.Camera1.Humidity:0}%"
            : "-";
            BedroomHumidityText = data?.Camera2?.Humidity != null
                ? $"{data.Camera2.Humidity:0}%"
                : "-";
            LedState = data?.Camera1?.LedState;
        }

        public ICommand SetTemperatureCommand => new Command(async () =>
        {
            await _apiService.SetTemperatureAsync(Temperature);
       //     await _apiService.AddEventLogAsync("SetTemperature", $"Temperatura setata la {Temperature}°C");
            MessagingCenter.Send(this, "TemperatureSet");
        });

      

        public string LightsIcon => LedState == true ? "power_on.png" : "power_off.png";
        public string LightsStatusText => LedState == true ? "Lights are on" : "Lights are off";
        private bool _lightsOn;
        public bool LightsOn
        {
            get => _lightsOn;
            set
            {
                if (_lightsOn != value)
                {
                    _lightsOn = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LightsIcon));
                    OnPropertyChanged(nameof(LightsStatusText));
                }
            }
        }


        public ICommand ToggleLightsCommand => new Command(async () =>
        {
            if (LightsOn)
            {
                await _apiService.TurnLightOffAsync();
                LightsOn = false;
               // await _apiService.AddEventLogAsync("Lightoff", "Lights turned off in camera1");
            }
            else
            {
                await _apiService.TurnLightOnAsync();
                LightsOn = true;
              //  await _apiService.AddEventLogAsync("Lighton", "Lights turned on in camera1");
            }
        });

        private bool? _ledState;
        public bool? LedState
        {
            get => _ledState;
            set
            {
                if (_ledState != value)
                {
                    _ledState = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(LightsIcon));
                    OnPropertyChanged(nameof(LightsStatusText));
                }
            }
        }

        private int _temperature = 22;
        public int Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TemperatureLabel));
                }
            }
        }

        public string TemperatureLabel => $"{Temperature}°C";

        private bool _isInside = false;
        public bool IsInside
        {
            get => _isInside;
            set
            {
                if (_isInside != value)
                {
                    _isInside = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AccessButtonText));
                }
            }
        }

        private bool _doorIsOpen = false;
        public bool DoorIsOpen
        {
            get => _doorIsOpen;
            set
            {
                if (_doorIsOpen != value)
                {
                    _doorIsOpen = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DoorStatusMessage));
                    OnPropertyChanged(nameof(DoorStatusColor));
                    CanAccessDoor = !_doorIsOpen;
                    DoorAccessCommand.ChangeCanExecute();
                }
            }
        }

        private bool _canAccessDoor = true;
        public bool CanAccessDoor
        {
            get => _canAccessDoor;
            set
            {
                if (_canAccessDoor != value)
                {
                    _canAccessDoor = value;
                    OnPropertyChanged();
                    DoorAccessCommand.ChangeCanExecute();
                }
            }
        }

        // Door status message and color
        public string DoorStatusMessage => DoorIsOpen ? "The door is open" : "The door is closed";
        public Color DoorStatusColor => DoorIsOpen ? Colors.Green : Colors.Gray;

        // Door access button text
        public string AccessButtonText => IsInside ? "Do you want to exit?" : "Do you want to enter?";

        // DoorAccessCommand with CanExecute
        private Command _doorAccessCommand;
        public Command DoorAccessCommand => _doorAccessCommand ??= new Command(async () =>
        {
            try
            {

                CanAccessDoor = false;
                    await _apiService.UnlockDoorAsync();
                DoorIsOpen = true;
               // await _apiService.AddEventLogAsync("Door Unlocked", "The door has been unlocked remotely.");
                await Application.Current.MainPage.DisplayAlert("Success", "Door unlocked!", "OK");

                // Auto-lock after 10 seconds
                await Task.Delay(10000);
                DoorIsOpen = false;
               // await _apiService.AddEventLogAsync("Door Locked", "The door has been locked automatically.");
                await Application.Current.MainPage.DisplayAlert("Info", "Door locked automatically.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to unlock door: {ex.Message}", "OK");
                CanAccessDoor = true;
            }
        }, () => CanAccessDoor);

        public ICommand NavigateCommand => new Command<string>(async (route) =>
        {
            if (!string.IsNullOrEmpty(route))
                await Shell.Current.GoToAsync($"//{route}", true);
        });
    }
}