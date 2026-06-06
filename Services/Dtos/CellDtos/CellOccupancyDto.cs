using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.CellDtos
{
    public class CellOccupancyDto
    {
        public int CellIndex { get; set; }
        public double FreeCapacity { get; set; }
    }
}
