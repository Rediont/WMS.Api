using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// в розробці

namespace Services.Dtos
{
    public class OutboundShipmentDto
    {
        public int Id { get; set; }

        public int ContractId { get; set; }

        public DateTime ShipmentDate { get; set; }

        public List<int> ShippedPalletIds { get; set; }

    }
}
