using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace api.Dto
{
    public class SmartHomePayload
    {
        [JsonPropertyName("camera1")]
        public CameraData Camera1 { get; set; }

        [JsonPropertyName("camera2")]
        public CameraData Camera2 { get; set; }

        [JsonPropertyName("datetime")]

        public string Datetime { get; set; }

        [JsonPropertyName("LockState")]
        public string LockState { get; set; }
    }
}