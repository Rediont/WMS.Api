using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.PalletBinding
{
    public class PalletBindingDictDto
    {
        public int AlleyIndex { get; set; }
        public Dictionary<int, int[]> CellPalletDict { get; set; }
    }
}
