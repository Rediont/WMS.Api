using Domain.Interface;

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

    public class Contract : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public ContractStatus CurrentStatus { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public ICollection<WmsDocument> Documents { get; set; } = new List<WmsDocument>();

    }
}