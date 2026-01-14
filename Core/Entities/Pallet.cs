using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pallet
    {
        public string? Id { get; set; }

        public int InboundReceiptId { get; set; }
        public virtual InboundReceipt InboundReceipt { get; set; }

        public int PalletTypeId { get; set; }
        public virtual PalletTypes PalletType { get; set; }

        public int? AlleyId { get; set; }
        public int? CellId { get; set; }
        public virtual Cell? Cell { get; set; } // Додано об'єкт
    }
}
