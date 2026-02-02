using Domain.Entities;
using Domain.Interface;

namespace Domain.Entities
{
    public class InboundReceipt : IEntity
    {
        public int Id { get; set; }
        
        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }
        
        public DateTime ReceiptDate { get; set; }
        
        public int PalletTypeId { get; set; }
        public virtual PalletTypes PalletType { get; set; }

        public int Amount { get; set; }

        public ICollection<Pallet> Pallets { get; set; } = new List<Pallet>();
    }
}