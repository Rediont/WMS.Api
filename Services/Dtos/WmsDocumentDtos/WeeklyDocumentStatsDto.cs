using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WmsDocumentDtos
{
    public class WeeklyDocumentStatsDto
    {
        public List<string> Dates { get; set; } = new();
        public List<int> Arrivals { get; set; } = new();
        public List<int> Departures { get; set; } = new();
    }
}
