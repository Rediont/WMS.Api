using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IItemRepository
    {

        public List<Item> GetAllItems();

        public List<Item> GetItemsByContractId(string contractId);

        public List<Item> GetItemsByClientId(string clientId);

        public void AddItem(Item item);

        public void RemoveItem(int itemId);

    }
}
