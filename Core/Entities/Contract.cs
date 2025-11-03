namespace Core.Entities
{
    public class Contract
    {
        public readonly string id;
        public readonly DateTime creation_date;
        public readonly DateTime expiration_date;

        public List<Contract_Shipment> Shipment_list;

    }
}