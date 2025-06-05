#if ANDROID
using Org.Apache.Http.Client;
#endif
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeIQ.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress = "http://192.168.178.62:5033";
        private string _authToken;
        public string AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                if (!string.IsNullOrEmpty(_authToken))
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken);
                else
                    _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public ApiService()
        {
            _httpClient = new HttpClient();
            // Poți seta baza URL aici dacă ai una comună:
            // _httpClient.BaseAddress = new Uri("https://api.exemplu.com/");
        }

        // Citire temperaturi și alte date de la backend
        public async Task<TemperatureResponse> GetCurrentTemperatureAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/api/CurrentTemperature");
            var status = response.StatusCode;
            if (status != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var result = JsonSerializer.Deserialize<TemperatureResponse>(content);
            return result;
           
        }

        public async Task SetTemperatureAsync(int temperature)
        {
            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(new { temperature = temperature }),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync($"{_baseAddress}/api/Temperature/set", content);
            response.EnsureSuccessStatusCode();
        }
        public async Task TurnLightOnAsync()
        {
            var response = await _httpClient.PostAsync($"{_baseAddress}/api/Light/on", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task TurnLightOffAsync()
        {
            var response = await _httpClient.PostAsync($"{_baseAddress}/api/Light/off", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task UnlockDoorAsync()
        {
            var response = await _httpClient.PostAsync($"{_baseAddress}/api/DoorHandler/unlock", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<NewUserDto> LoginAsync(string username, string password)
        {
            var loginDto = new LoginDto { Username = username, Password = password };
            var json = System.Text.Json.JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await _httpClient.PostAsync($"{_baseAddress}/api/account/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return System.Text.Json.JsonSerializer.Deserialize<NewUserDto>(responseContent);
                }
                else
                {
                    // Optionally, handle error messages here
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login exception: {ex.Message}");
                return null;
            }
        }

        public async Task<List<TemperatureProgramDto>> GetProgramsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/api/temperature-programs");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TemperatureProgramDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task ActivateProgramAsync(String name)
        {
            var response = await _httpClient.PostAsync($"{_baseAddress}/api/temperature-programs/select/{name}", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddEventLogAsync(string eventType, string details = null)
        {
            var dto = new EventLogDto { EventType = eventType, Details = details };
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseAddress}/api/eventlog", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<EventLog>> GetLastEventLogsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseAddress}/api/eventlog/last");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EventLog>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Trimitere comandă către backend (ex: aprindere lumini, schimbare temperatură)
        public async Task SendCommandAsync(string command, object parameters = null)
        {
            var payload = new
            {
                Command = command,
                Parameters = parameters
            };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.exemplu.com/commands", content);
            response.EnsureSuccessStatusCode();
        }
    }

    // Modele pentru deserializare răspuns backend
    public class TemperatureResponse
    {
        [JsonPropertyName("datetime")]
        public string Datetime { get; set; }

        [JsonPropertyName("camera1")]
        public CameraData Camera1 { get; set; }

        [JsonPropertyName("camera2")]
        public CameraData Camera2 { get; set; }

        [JsonPropertyName("lockState")]
        public string LockState { get; set; } // string, nu bool!
    }

    public class CameraData
    {
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("ledState")]
        public bool? LedState { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class NewUserDto
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }

    public class TemperatureIntervalDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("programId")]
        public int ProgramId { get; set; }

        [JsonPropertyName("start")]
        public TimeSpan Start { get; set; }

        [JsonPropertyName("end")]
        public TimeSpan End { get; set; }

        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }
    }

    public class TemperatureProgramDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("intervals")]
        public List<TemperatureIntervalDto> Intervals { get; set; }
    }

    public class EventLogDto
    {
        [JsonPropertyName("eventType")]
        public string EventType { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }
    }

    public class EventLog
    {
        [JsonPropertyName("eventType")]
        public string EventType { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }


}