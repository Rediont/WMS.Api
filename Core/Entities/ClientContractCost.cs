using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ClientContractCost
    {
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        
        public int ContaractId { get; set; }
        public virtual Contract Contract { get; set; }
        
        public int PalletTypeId { get; set; }
        public virtual PalletTypes PalletType { get; set; }

        public int Cost { get; set; }
    }
}
