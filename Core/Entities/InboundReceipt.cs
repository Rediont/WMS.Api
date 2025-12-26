using Domain.Entities;

namespace Domain.Entities
{
    public class InboundReceipt
    {
        public int id;
        public int contractId;
        public DateTime receiptDate;
        public int palletTypeId;
        public int amount;

        public PalletTypes palletType;
        public List<Pallet> pallets;
    }
}