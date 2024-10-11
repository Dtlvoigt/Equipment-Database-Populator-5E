using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EquipmentDatabasePopulator5E.Models
{
    public class WeaponProperty
    {
        [Key]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [NotMapped]
        [JsonPropertyName("desc")]
        public JsonElement? DescriptionElement { get; set; }
        public string? Description
        {
            get
            {
                if (DescriptionElement.HasValue && DescriptionElement.Value.ValueKind == JsonValueKind.Array)
                {
                    // extract all strings from the array and join them with a delimiter
                    return string.Join("; ", DescriptionElement.Value.EnumerateArray().Select(d => d.GetString()));
                }
                else
                {
                    return null;
                }
            }
            set { }
        }
    }
}
