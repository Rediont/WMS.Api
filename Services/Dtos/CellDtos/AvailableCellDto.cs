using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.CellDtos
{
    public class AvailableCellDto
    {
        public int CellId { get; set; }
        public int AlleyIndex { get; set; }
        public int FloorIndex { get; set; }
        public double AvailableCapacity { get; set; }
    }
}
