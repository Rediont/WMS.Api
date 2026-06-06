using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ContractDtos
{
    public class ContractDetailsDto
    {
        public int Id { get; set; }
        public string ContractName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public ContractStatus CurrentStatus { get; set; }

        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }

        public IEnumerable<RelatedDocumentDto>? Documents { get; set; }

    }

    public class RelatedDocumentDto
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public DateTime CreationDate { get; set; }
        public int ExpectedTotalPallets { get; set; }
    }
}
