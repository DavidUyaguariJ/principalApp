using System.Text.Json.Serialization;

namespace HumanTalentApp.Models
{
    public class JwtInfo
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
