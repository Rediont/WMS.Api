
namespace Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string EDRPO { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }

        public List<Contract> ContractList = new();
    }
}
