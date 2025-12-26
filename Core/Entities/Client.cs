
namespace Domain.Entities
{
    public class Client
    {
        public int id;
        public string EDRPO;
        public string name;
        public string email;
        public string contactPersonName;
        public string contactPersonPhone;

        public List<Contract> ContractList;
    }
}
