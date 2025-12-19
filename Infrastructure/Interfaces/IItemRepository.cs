using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        public List<Item> GetItemsByContractId(int contractId);

        public List<Item> GetItemsByClientId(int clientId);
    }
}
