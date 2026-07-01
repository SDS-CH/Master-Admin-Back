using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs
{
    public class ProductServiceCategoryDto
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int? IndustryId { get; set; }
        public bool IsDefault { get; set; }
    }
}
