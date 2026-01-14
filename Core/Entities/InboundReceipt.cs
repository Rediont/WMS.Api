using Domain.Entities;

namespace Domain.Entities
{
    public class InboundReceipt
    {
        public int Id { get; set; }
        
        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }
        
        public DateTime ReceiptDate { get; set; }
        
        public int PalletTypeId { get; set; }
        public virtual PalletTypes PalletType { get; set; }

        public int Amount { get; set; }

        public List<Pallet> Pallets = new();
    }
}