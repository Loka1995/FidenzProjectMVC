using System.Text.Json.Serialization;

namespace FidenzProjectMVC.Models
{
    public class Address
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("state")]
        public string? State { get; set; }

        [JsonPropertyName("zipcode")]
        public int ZipCode { get; set; }
    }
}
