using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.CellDtos
{
    public class AlleyCellOccupancyMapDto
    {
        public int AlleyIndex { get; set; }
        public int FloorIndex { get; set; }

        public List<CellOccupancyDto> CellOccupancies { get; set; } = new List<CellOccupancyDto>();
    }
}
