using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WmsDocumentDtos
{
    public class NewDocumentDto
    {
        public int documentTypeId { get; set; }
        public int contractId { get; set; }
        public NewDocumentItemsDto items { get; set; }
    }
}
