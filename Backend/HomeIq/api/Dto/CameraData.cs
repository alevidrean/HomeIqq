using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace api.Dto
{
    public class CameraData
    {

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("ledstate")]
        public bool? LedState { get; set; }
    }
}