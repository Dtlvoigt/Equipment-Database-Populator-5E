using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentDatabasePopulator5E.Models
{
    public class EquipmentVariant
    {
        public required int ParentEquipmentId { get; set; }
        public Equipment? ParentEquipment { get; set; } = null;

        public required int VariantId { get; set; }
        public Equipment? Variant { get; set; } = null;
    }
}
