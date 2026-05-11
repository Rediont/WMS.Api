using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.PalletDtos
{
    public class PalletInfoDto
    {
        public int Id { get; set; }

        public int ArrivalDocumentId { get; set; }

        public DateTime ArrivalDate { get; set; }

        public int PalletTypeId { get; set; }
        public string PalletTypeName { get; set; }

        public int? AlleyIndex { get; set; }
        public int? CellIndex { get; set; }
    }
}
