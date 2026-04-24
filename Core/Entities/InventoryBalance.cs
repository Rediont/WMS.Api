using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InventoryBalance : IEntity
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public int PalletTypeId { get; set; }
        public virtual PalletType PalletType { get; set; }

        public int Quantity { get; set; }

    }
}
