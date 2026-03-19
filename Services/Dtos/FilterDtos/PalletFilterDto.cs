using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.FilterDtos
{
    public class PalletFilterDto
    {
        public int? ContractId { get; set; }
        public int? PalletType { get; set; }
        public int? alleyId { get; set; }
    }
}
