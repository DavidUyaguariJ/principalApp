using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HumanTalentApp.Models
{
    public class MessageModel
    {
        [JsonPropertyName("idClient")]
        public string IdClient { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }
        [JsonPropertyName("productQuantity")]
        public int ProductQuantity { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
