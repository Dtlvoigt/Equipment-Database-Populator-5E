using System.ComponentModel.DataAnnotations;

namespace EquipmentDatabasePopulator5E.Models
{
    public class EquipmentWeaponProperty
    {
        public required int EquipmentId { get; set; }
        public Equipment? Equipment { get; set; } = null;

        public required int WeaponPropertyId { get; set; }
        public WeaponProperty? WeaponProperty { get; set; } = null;
    }
}
