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
        public required int EquipmentId { get; set; }
        public Equipment? Equipment { get; set; } = null;

        public required int VariantId { get; set; }
        public Equipment? Variant { get; set; } = null;
    }
}
