using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InventoryBalance : IEntity
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }
        public virtual WmsDocument Document { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public int PalletTypeId { get; set; }
        public virtual PalletType PalletType { get; set; }

        public int Amount { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public int BatchDocumentId { get; set; }
        public virtual WmsDocument BatchDocument { get; set; }

    }
}
