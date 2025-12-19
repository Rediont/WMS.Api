namespace Core.Entities
{
    public enum ContractStatus
    {
        Inactive,
        Active,
        Terminated,
        Completed,
        invalid
    }

    public class Contract
    {
        public int id;
        public string name;
        public DateTime start_date;
        public DateTime expiration_date;

        public ContractStatus current_status;

        public List<ContractShipment> shipment_list; // List of shipments associated with the contract
        public List<ContractDelivery> dispatch_list; // List of arrivals associated with the contract

        public List<Item> item_list; // List of items associated with the contract

    }
}