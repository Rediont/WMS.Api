using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class InbundReceiptPalletsInfoDto
    {
        public int InboundReceiptId { get; set; }
        public List<PalletInfoDto> Pallets { get; set; }
    }
}
