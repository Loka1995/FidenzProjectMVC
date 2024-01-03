using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FidenzProjectMVC.Models.Dto
{
    public class EditUserDto
    {
        [Key]
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
    }
}
