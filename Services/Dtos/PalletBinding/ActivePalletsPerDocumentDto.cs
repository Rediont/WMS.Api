using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.PalletBinding
{
    public class ActivePalletsPerDocumentDto
    {
        public int DocumentId { get; set; }
        public Dictionary<int, int> ActivePalletsCount { get; set; } // словник тип-кількість палет для кожного типу палет в документі

        public int TotalActivePallets => ActivePalletsCount.Values.Sum();
    }
}
