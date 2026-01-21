using Domain.Entities;

// треба придумати як реалізувати вивантаження товарів
// клієнт може захотіти вивантаження різних палет
// пропозиція робити за FIFO

namespace Domain.Entities
{
    public class OutboundShipment
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public DateTime ShipmentDate { get; set; }

        public List<Item> ShippedItems = new();
    }
}