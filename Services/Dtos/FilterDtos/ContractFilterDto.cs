using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.FilterDtos
{
    public class ContractFilterDto
    {
        public int[]? ClientIds { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public ContractStatus? Status { get; set; }
    }
}
