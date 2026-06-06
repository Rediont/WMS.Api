using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WmsDocumentDtos
{
    public class DocumentDetailsDto
    {
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }

        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public int ContractId { get; set; }
        public string ContractName { get; set; }

        public int TotalItems { get; set; }
        public int DocumentType { get; set; }

        public DateTime CreationDate { get; set; }

        public List<DocumentDetailsItemDto> Items { get; set; }
    }

    public class DocumentDetailsItemDto
    {
        public int Id { get; set; }
        public int ExpectedAmount { get; set; }
        public int PalletTypeId { get; set; }
    }

}
