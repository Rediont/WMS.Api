using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.PaymentDto
{
    public class BillRequestDto
    {
        public int ClientId { get; set; }
        public int ContractId { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
