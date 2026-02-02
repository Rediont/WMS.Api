
using Domain.Interface;

namespace Domain.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }
        public string EDRPO { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }

        public ICollection<Contract> ContractList { get; set; } = new List<Contract>();
    }
}
