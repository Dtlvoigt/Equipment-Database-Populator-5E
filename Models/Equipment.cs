using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentDatabasePopulator5E.Models
{
    public class Equipment
    {

        /////////////////////
        //shared properties//
        /////////////////////

        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? URL { get; set; }
        public string? Category { get; set; }
        public bool? MagicItem { get; set; }
        public string? Description { get; set; }
        public double Cost { get; set; }
        public double Weight { get; set; }

        /////////////////////
        //weapon properties//
        /////////////////////

        public string? WeaponCategory { get; set; }
        public string? WeaponRange { get; set; }
        public string? RangeCategory { get; set; }
        public int? RangeNormal { get; set; }
        public int? RangeLong { get; set; }
        public int? ThrowRangeNormal { get; set; }
        public int? ThrowRangeLong { get; set; }
        public string? DamageDice { get; set; }
        public string? DamageType { get; set; }
        public string? TwoHandedDamageDice { get; set; }
        public string? TwoHandedDamageType { get; set; }
        public string? SpecialAttribute { get; set; }
        //public string WeaponProperties { get; set; }
        public ICollection<EquipmentWeaponProperty>? WeaponProperties { get; set; }

        ////////////////////
        //armor properties//
        ////////////////////

        public string? ArmorCategory { get; set; }
        public int? ArmorClass { get; set; }
        public bool? DexBonus { get; set; }
        public int? MaxDexBonus { get; set; }
        public int? StrengthMinimum { get; set; }
        public bool? StealthDisadvantage { get; set; }

        ///////////////////
        //gear properties//
        ///////////////////

        public string? GearCategory { get; set; }
        //public string? PackContents { get; set; }
        public ICollection<PackContent>? PackContents { get; set; }

        ///////////////////
        //tool properties//
        ///////////////////
        public string? ToolCategory { get; set; }

        //////////////////////
        //vehicle properties//
        //////////////////////

        public string? VehicleCategory { get; set; }
        public string? Speed { get; set; }

        /////////////////////////
        //magic item properties//
        /////////////////////////

        public string? Rarity { get; set; }
        public string? ImageURL { get; set; }
        //public string? Variants {  get; set; }
        public bool IsVariant { get; set; }
        public bool HasVariant { get; set; }
        public int? ParentEquipmentId { get; set; }
        public Equipment? ParentEquipment { get; set; }
        public ICollection<Equipment>? Variants { get; set; }
        //public ICollection<EquipmentVariant>? Variants { get; set; }
    }
}
