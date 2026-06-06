using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.PaymentDto
{
    public class BillItemDto
    { 
        public int PalletTypeId { get; set; }
        public string PalletTypeName { get; set; }
        public int AmountOfDays { get; set; }
        public decimal CostPerDay { get; set; }
        public decimal TotalCost { get; set; } 
    }
}
