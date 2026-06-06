using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.LookupDtos
{
    public class WarehouseSettingsLookupDto
    {
        public int NumberOfAlleys { get; set; }
        public int NumberOfAlleyFloors { get; set; }
        public int NumberOfCellsInAlley { get; set; }
        public int NumberOfCellsInAlleyFloor { get; set; }
        public int NumberOfCells { get; set; }
    }
}
