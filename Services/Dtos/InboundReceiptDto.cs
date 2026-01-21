using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class InboundReceiptDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int PalletTypeId { get; set; }

        public int Amount { get; set; }
    }
}
