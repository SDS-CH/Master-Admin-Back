using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Entities.Models
{
    public class ProductServiceCategory
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int? TenantId { get; set; }
        public int IndustryId { get; set; }
        public bool IsDefault { get; set; }
    }
}
