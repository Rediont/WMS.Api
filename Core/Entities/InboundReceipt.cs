using Domain.Entities;

namespace Domain.Entities
{
    public class InboundReceipt
    {
        public int id;
        public int contractId;
        public DateTime receiptDate;
        
        public List<Item> recievedItems;
    }
}