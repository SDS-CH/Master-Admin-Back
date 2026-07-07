using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.DTO.DTOs
{
    public class CreateCustomFieldDto
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool IsRequiredOnFileClosure { get; set; }
        public string CodeTypeDossier { get; set; }
    }
}