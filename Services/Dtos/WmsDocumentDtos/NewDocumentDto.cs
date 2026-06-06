using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WmsDocumentDtos
{
    public class NewDocumentDto
    {
        public int DocumentTypeId { get; set; }
        public int ClientId { get; set; }
        public int ContractId { get; set; }
        public DateTime CreationDate { get; set; }
        public NewDocumentItemsDto Items { get; set; }
    }
}
