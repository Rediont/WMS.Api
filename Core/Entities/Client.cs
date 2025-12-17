
namespace Core.Entities
{
    public class Client
    {
        public Guid id;
        public string name;
        public string email;
        public string contact_person_name;
        public string contact_phone;

        public List<Contract> Contract_list;
    }
}
