namespace Core.Entities
{
    public enum ContractStatus
    {
        Active,
        Terminated,
        Completed,
        invalid
    }

    public class Contract
    {
        public readonly string id;
        public readonly DateTime creation_date;
        public readonly DateTime expiration_date;

        public ContractStatus status;

        public List<Contract_Shipment> Shipment_list; // List of shipments associated with the contract
        public List<Contract_Arrival> Dispatch_list; // List of arrivals associated with the contract

        public List<Item> Item_list; // List of items associated with the contract

    }
}