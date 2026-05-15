using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.PaymentDto
{
    public class BillDto
    {
        public int ClientId { get; set; }
        public  string ClientName { get; set; }

        public int ContractId { get; set; }
        public string ContractName { get; set; }

        public List<BillItemDto> Items { get; set; } 

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public decimal TotalCost { get; set; }
    }
}
