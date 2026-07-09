using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

using System.Collections.Generic;

using System.Collections.Generic;

namespace DMS.DTO.DTOs
{
    public class LinkCustomFieldItemDto
    {
        public string ComplementCode { get; set; }
        public bool IsRequiredOnFileClosure { get; set; }
    }

    public class LinkCustomFieldDto
    {
        public string CodeTypeDossier { get; set; }
        public List<LinkCustomFieldItemDto> Items { get; set; }
    }
}
