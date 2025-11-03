
namespace Core.Entities
{
    public class Client
    {
        public readonly string id;
        public readonly string name;
        public readonly string contact_person_name;
        public readonly string contact_phone;

        public List<Contract> Contract_list;
    }
}
