using Domain.Interface;

namespace Domain.Entities
{

    public enum DocumentType
    {
        OutboundShipment,
        InboundReceipt,
        InventoryAdjustment,
        TransferOrder,
        Other
    }


    public class WmsDocument : IEntity
    {
        public int Id { get; set; }
        
        public DateTime CreationDate { get; set; }

        public  DocumentType DocumentType { get; set; }

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public ICollection<WmsDocumentItem> Items { get; set; } = new List<WmsDocumentItem>();

        public ICollection<Pallet> Pallets { get; set; } = new List<Pallet>();
    }
}
