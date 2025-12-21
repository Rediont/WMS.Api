using Domain.Entities;

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