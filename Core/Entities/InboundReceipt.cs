using Domain.Entities;

namespace Domain.Entities
{
    public class InboundReceipt
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int PalletTypeId { get; set; }
        public int Amount { get; set; }

        public virtual PalletTypes palletType { get; set; }
        public List<Pallet> pallets = new();
    }
}