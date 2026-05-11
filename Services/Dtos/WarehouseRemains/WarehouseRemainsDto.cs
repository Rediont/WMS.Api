using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WarehouseRemains
{
    public class WarehouseRemainsDto
    {   
        public int DocumentId { get; set; }
        public string DocumentName { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }

        public int? ContractId { get; set; }
        public string ContractName { get; set; }

        public int? PalletTypeId { get; set; }
        public string PalletTypeName { get; set; }

        public DateTime TransactionDate { get; set; }

        public int Amount { get; set; }

        public int BatchDocumentId { get; set; }
        public string BatchDocumentName { get; set; }
    }
}
