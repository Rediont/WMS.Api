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
    internal class ItemRepo : IItemRepository
    {
        private readonly DbContext _dbContext;

        public List<Item> GetAllItems()
        {
            return _dbContext.Items.ToList();
        }

        public List<Item> GetItemsByContractId(string contractId)
        {
            if (string.IsNullOrEmpty(contractId))
            {
                throw new ArgumentException("contractId cannot be null or empty", nameof(contractId));
            }

            if (!_dbContext.Contracts.Any(c => c.id == contractId))
            {
                throw new InvalidOperationException($"Contract with ID {contractId} does not exist.");
            }

            return _dbContext.Contracts
                .Where(c => c.id == contractId)
                .SelectMany(c => c.Item_list)
                .ToList();
        }

        public List<Item> GetItemsByClientId(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("clientId cannot be null or empty", nameof(clientId));
            }

            if (!_dbContext.Clients.Any(cl => cl.id == clientId))
            {
                throw new InvalidOperationException($"Client with ID {clientId} does not exist.");
            }

            return _dbContext.Clients
                .Where(cl => cl.id == clientId)
                .SelectMany(cl => cl.Contract_list)
                .SelectMany(c => c.Item_list)
                .ToList();
        }

        public void AddItem(Item item)
        {
            if (_dbContext.Items.Contains(item))
            {
                throw new Exception("Item already exists");
            }
            this._dbContext.Items.Add(item);
        }

        public void RemoveItem(int itemId)
        {
            Item? targetItem = this._dbContext.Items.Find(itemId);
            if (targetItem == null)
            {
                throw new Exception("Item not found");
            }
            _dbContext.Items.Remove(targetItem);
            _dbContext.SaveChanges();
        }

    }
}
