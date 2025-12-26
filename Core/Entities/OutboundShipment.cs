using Domain.Entities;

// треба придумати як реалізувати вивантаження товарів
// клієнт може захотіти вивантаження різних палет
// пропозиція робити ла FIFO

namespace Domain.Entities
{
    public class OutboundShipment
    {
        public int id;
        public int contractId;
        public DateTime shipmentDate;
        
        public List<Item> ShippedItems;
    }
}