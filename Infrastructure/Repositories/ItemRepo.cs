using Core.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class ItemRepo : Repository<Item>, IItemRepository
    {
        private readonly DbContext _dbContext;


        public List<Item> GetItemsByContractId(int contractId)
        {
            if (!_dbContext.Contracts.Any(c => c.id == contractId))
            {
                throw new InvalidOperationException($"Contract with ID {contractId} does not exist.");
            }

            return _dbContext.Contracts
                .Where(c => c.id == contractId)
                .SelectMany(c => c.item_list)
                .ToList();
        }

        public List<Item> GetItemsByClientId(int clientId)
        {
            if (!_dbContext.Clients.Any(cl => cl.id == clientId))
            {
                throw new InvalidOperationException($"Client with ID {clientId} does not exist.");
            }

            return _dbContext.Clients
                .Where(cl => cl.id == clientId)
                .SelectMany(cl => cl.Contract_list)
                .SelectMany(c => c.item_list)
                .ToList();
        }
    }
}
