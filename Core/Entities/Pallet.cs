using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{

    public enum PalletStatus
    {
        Arrived,
        Arranged,
        Stored,
        Shipped
    }

    public class Pallet : IEntity
    {
        public int Id { get; set; }

        public int ArrivalDocumentId { get; set; }
        public virtual WmsDocument ArrivalDocument { get; set; }

        public int PalletTypeId { get; set; }
        public virtual PalletType PalletType { get; set; }

        public PalletStatus PalletStatus { get; set; }

        public int? AlleyIndex { get; set; }
        public int? CellIndex { get; set; }
        public virtual Cell? Cell { get; set; } // Додано об'єкт
    }
}
