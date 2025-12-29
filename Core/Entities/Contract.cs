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

    public class Contract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public ContractStatus CurrentStatus;

        public List<OutboundShipment> Outbounds = new(); // List of shipments associated with the contract
        public List<InboundReceipt> Inbounds = new(); // List of arrivals associated with the contract

        //public List<Item> itemList; // List of items associated with the contract

    }
}