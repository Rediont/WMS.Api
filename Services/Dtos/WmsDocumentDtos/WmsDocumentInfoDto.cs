using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WmsDocumentDtos
{
    public class WmsDocumentInfoDto
    {
        public int Id { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ContractId { get; set; }
        public string ContractName { get; set; }

        public DateTime CreationDate { get; set; }
        public IEnumerable<WmsDocumentItemDto> Items { get; set; }
    }
}
