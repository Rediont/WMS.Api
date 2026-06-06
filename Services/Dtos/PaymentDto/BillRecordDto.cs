using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.PaymentDto
{
    public class BillRecordDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ContractId { get; set; }
        public string ContractName { get; set; }

        public DateTime CreationDate { get; set; }

        public double Total { get; set; }

        public bool IsPaid { get; set; }
    }
}
