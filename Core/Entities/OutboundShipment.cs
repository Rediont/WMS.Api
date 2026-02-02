using Domain.Entities;
using Domain.Interface;

// треба придумати як реалізувати вивантаження товарів
// клієнт може захотіти вивантаження різних палет
// пропозиція робити за FIFO

namespace Domain.Entities
{
    public class OutboundShipment : IEntity
    {
        public int Id { get; set; }

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public DateTime ShipmentDate { get; set; }

        public ICollection<Pallet> ShippedPallets { get; set; } = new List<Pallet>();
    }
}