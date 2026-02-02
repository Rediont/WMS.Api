using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class PalletInfoDto
    {
        public int Id { get; set; }

        public int InboundReceiptId { get; set; }
        public DateTime ArrivalDate { get; set; }

        public int PalletTypeId { get; set; }
        public string PalletTypeName { get; set; }

        public int? AlleyId { get; set; }
        public int? CellId { get; set; }
    }
}
