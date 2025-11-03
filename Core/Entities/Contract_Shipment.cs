using Core.Entities;

namespace Core.Entities
{
    public class Contract_Shipment
    {
        public readonly string id;
        public readonly string contract_id;
        public readonly DateTime arrival_date;
        
        public List<Item> Item_list;
    }
}