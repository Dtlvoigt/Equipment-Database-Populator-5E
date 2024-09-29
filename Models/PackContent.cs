using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EquipmentDatabasePopulator5E.Models
{
    public class PackContent
    {
        public required int PackId { get; set; }
        public Equipment? PackEquipment { get; set; } = null;

        public required int ContentId { get; set; }
        public Equipment? ContentEquipment { get; set; } = null;

        public int Amount { get; set; }
    }
}
