using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class WarehouseSettingsDto
    {
        public int NumberOfAlleys { get; set; }
        public int NumberOfFloorsPerAlley { get; set; }
        public int CellsPerAlleyFloor { get; set; }
    }
}
