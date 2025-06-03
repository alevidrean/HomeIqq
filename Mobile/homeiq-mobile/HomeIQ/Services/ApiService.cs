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

        public ApiService()
        {
            _httpClient = new HttpClient();
            // Poți seta baza URL aici dacă ai una comună:
            // _httpClient.BaseAddress = new Uri("https://api.exemplu.com/");
        }

        // Citire temperaturi și alte date de la backend
        public async Task<TemperatureResponse> GetCurrentTemperatureAsync()
        {
            var response = await _httpClient.GetAsync("http://192.168.100.168:5033/api/CurrentTemperature");
            var status = response.StatusCode;
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
            var response = await _httpClient.PostAsync("http://192.168.100.168:5033/api/Temperature/set", content);
            response.EnsureSuccessStatusCode();
        }
        public async Task TurnLightOnAsync()
        {
            var response = await _httpClient.PostAsync("http://192.168.100.168:5033/api/Light/on", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task TurnLightOffAsync()
        {
            var response = await _httpClient.PostAsync("http://192.168.100.168:5033/api/Light/off", null);
            response.EnsureSuccessStatusCode();
        }

        public async Task<NewUserDto> LoginAsync(string username, string password)
        {
            var loginDto = new LoginDto { Username = username, Password = password };
            var json = System.Text.Json.JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://192.168.100.168:5033/api/account/login", content);

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
}