using Core.Entities;

namespace Core.Entities
{
    public class ContractDelivery
    {
        public readonly int id;
        public int contract_id;
        public DateTime arrival_date;
        
        public List<Item> item_list;
    }
}