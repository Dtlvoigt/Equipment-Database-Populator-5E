using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EquipmentDatabasePopulator5E.Models
{
    public class EquipmentCategory
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public required string Name { get; set; }
    }
}
