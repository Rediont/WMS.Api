using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class ContractShipmentsInfoDto
    {
        public int ContractId { get; set; }
        public List<OutboundShipmentDto> OutboundShipments { get; set; }
        public List<InboundReceiptDto> InboundReceipts { get; set; }
    }
}
