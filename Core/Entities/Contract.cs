using Domain.Interface;

namespace Domain.Entities
{
    public enum ContractStatus
    {
        Inactive,
        Active,
        Terminated,
        Completed,
        Invalid
    }

    public class Contract : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public ContractStatus CurrentStatus;

        public ICollection<OutboundShipment> Outbounds { get; set; } = new List<OutboundShipment>(); // List of shipments associated with the contract
        public ICollection<InboundReceipt> Inbounds { get; set; } = new List<InboundReceipt>(); // List of arrivals associated with the contract

        //public List<Item> itemList; // List of items associated with the contract

    }
}