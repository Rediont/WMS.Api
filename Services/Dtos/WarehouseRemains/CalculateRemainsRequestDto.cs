using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WarehouseRemains
{
    public class CalculateRemainsRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool GroupByClients { get; set; }
        public bool GroupByContracts { get; set; }
        public bool GroupByPalletTypes { get; set; }
    }
}
