using Core.Entities;

namespace Core.Entities
{
    public class ContractShipment
    {
        public readonly string id;
        public string contract_id;
        public DateTime shipment_date;
        
        public List<Item> item_list;
    }
}