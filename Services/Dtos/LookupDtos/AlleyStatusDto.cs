using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.LookupDtos
{
    public class AlleyStatusDto
    {
        public int Id { get; set; }
        public int OccupancyRate { get; set; } = 0;
    }
}
