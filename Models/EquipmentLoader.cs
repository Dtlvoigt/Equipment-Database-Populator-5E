using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/// <summary>
/// This model is designed to handle the deserialization and transformation of JSON data returned by the D&D 5e API
/// into a format suitable for use in an Entity Framework Core context.
///
/// The <c>JsonElement</c> properties are used for when the API returns a nested object.
/// The <c>EquipmentService</c> class then parses these elements for the desired information.
/// </summary>

namespace EquipmentDatabasePopulator5E.Models
{
    public class EquipmentLoader
    {

        /////////////////////
        //shared properties//
        /////////////////////

        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("url")]
        public string? URL { get; set; }
        [JsonPropertyName("equipment_category")]
        public JsonElement CategoryElement { get; set; }
        [JsonPropertyName("desc")]
        public JsonElement DescriptionElement { get; set; }
        [JsonPropertyName("cost")]
        public JsonElement CostElement { get; set; }
        [JsonPropertyName("weight")]
        public float Weight { get; set; }

        /////////////////////
        //weapon properties//
        /////////////////////

        [JsonPropertyName("weapon_category")]
        public string? WeaponCategory { get; set; }
        [JsonPropertyName("weapon_range")]
        public string? WeaponRange { get; set; }
        [JsonPropertyName("category_range")]
        public string? RangeCategory { get; set; }
        [JsonPropertyName("range")]
        public JsonElement RangeElement { get; set; }
        [JsonPropertyName("throw_range")]
        public JsonElement ThrowRangeElement { get; set; }
        [JsonPropertyName("damage")]
        public JsonElement DamageElement { get; set; }
        [JsonPropertyName("two_handed_damage")]
        public JsonElement TwoHandedElement { get; set; }
        [JsonPropertyName("special")]
        public JsonElement SpecialAttributeElement { get; set; }
        [JsonPropertyName("properties")]
        public JsonElement WeaponPropertiesElement { get; set; }

        ////////////////////
        //armor properties//
        ////////////////////

        [JsonPropertyName("armor_category")]
        public string? ArmorCategory { get; set; }
        [JsonPropertyName("armor_class")]
        public JsonElement ArmorClassElement { get; set; }
        [JsonPropertyName("str_minimum")]
        public int? StrengthMinimum { get; set; }
        [JsonPropertyName("stealth_disadvantage")]
        public bool? StealthDisadvantage { get; set; }

        ///////////////////
        //gear properties//
        ///////////////////

        [JsonPropertyName("gear_category")]
        public JsonElement GearCategoryElement { get; set; }
        [JsonPropertyName("contents")]
        public JsonElement PackContentsElement { get; set; }

        ///////////////////
        //tool properties//
        ///////////////////

        [JsonPropertyName("tool_category")]
        public string? ToolCategory { get; set; }

        //////////////////////
        //vehicle properties//
        //////////////////////

        [JsonPropertyName("vehicle_category")]
        public string? VehicleCategory { get; set; }
        [JsonPropertyName("speed")]
        public JsonElement SpeedElement { get; set; }

        /////////////////////////
        //magic item properties//
        /////////////////////////

        [JsonPropertyName("rarity")]
        public JsonElement RarityElement { get; set; }
        [JsonPropertyName("variants")]
        public JsonElement VariantsElement { get; set; }
        [JsonPropertyName("variant")]
        public bool IsVariant { get; set; }
    }
}
