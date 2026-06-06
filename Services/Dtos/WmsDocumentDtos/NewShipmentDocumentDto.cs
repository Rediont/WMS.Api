using Services.Dtos.PalletDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WmsDocumentDtos
{
    public class NewShipmentDocumentDto
    {
        public int ClientId { get; set; }
        public int ContractId { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<PalletInfoDto> pallets { get; set; }
    }
}
