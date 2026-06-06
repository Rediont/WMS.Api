using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BillItem : IEntity
    {
        public int Id { get; set; }

        public int BillId { get; set; }
        public virtual Bill Bill { get; set; }

        public int PalletTypeId { get; set; }
        public virtual PalletType PalletType { get; set; }

        public int AmountOfDays { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
