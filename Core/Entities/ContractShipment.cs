using Core.Entities;

namespace Core.Entities
{
    public class ContractShipment
    {
        public readonly int id;
        public int contract_id;
        public DateTime shipment_date;
        
        public List<Item> item_list;
    }
}