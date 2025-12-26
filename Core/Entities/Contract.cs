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
        public int id;
        public string name;
        public DateTime startDate;
        public DateTime expirationDate;

        public ContractStatus currentStatus;

        public List<OutboundShipment> Outbounds; // List of shipments associated with the contract
        public List<InboundReceipt> Inbounds; // List of arrivals associated with the contract

        //public List<Item> itemList; // List of items associated with the contract

    }
}