using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pallet
    {
        public int inboundReceiptId;
        public InboundReceipt inboundReceipt { get; set; }

        public int palletTypeId;
        public PalletTypes palletType;

        public int? alleyId;
        public int? cellId;
        public Cell? cell { get; set; } // Додано об'єкт

        public string? id;
    }
}
