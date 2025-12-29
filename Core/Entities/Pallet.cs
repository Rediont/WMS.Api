using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pallet
    {
        public int InboundReceiptId { get; set; }
        public virtual InboundReceipt InboundReceipt { get; set; }

        public int palletTypeId { get; set; }
        public virtual PalletTypes PalletType { get; set; }

        public int? alleyId { get; set; }
        public int? cellId { get; set; }
        public virtual Cell? cell { get; set; } // Додано об'єкт

        public string? id { get; set; }
    }
}
