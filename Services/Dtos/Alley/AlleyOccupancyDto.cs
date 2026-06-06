using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.Alley
{
    public class AlleyOccupancyDto
    {
        public int AlleyId { get; set; }
        public double OccupancyPercentage { get; set; } = 0;
    }
}
