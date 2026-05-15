using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.PalletDtos
{
    public class PalletAssignmentDto
    {
        public Dictionary<int, List<int>> CellPallets { get; set; } = new();
    }
}
