using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.CellDtos
{
    public class CellStatsDto
    {
        public int FreeCells { get; set; }
        public int OccupiedCells { get; set; }
        public int BlockedCells { get; set; }

        public int TotalCells { get; set; } 
    }
}
