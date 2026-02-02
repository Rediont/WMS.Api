using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class ClientContractCostDto
    {
        public int ClientId { get; set; }
        public int ContaractId { get; set; }
        public int PalletTypeId { get; set; }
        public int Cost { get; set; }
    }
}
