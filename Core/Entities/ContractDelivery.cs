using Core.Entities;

namespace Core.Entities
{
    public class ContractDelivery
    {
        public readonly string id;
        public string contract_id;
        public DateTime arrival_date;
        
        public List<Item> item_list;
    }
}