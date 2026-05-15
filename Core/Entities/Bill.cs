using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Bill : IEntity
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public decimal TotalCost { get; set; }

        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate  { get; set; }

        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; } = string.Empty;

        public virtual ICollection<BillItem> BillItems { get; set; } = new List<BillItem>();
    }
}
