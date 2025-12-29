using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    internal class CellStatusLog
    {
        public int AlleyId { get; set; }
        public int CellId { get; set; }
        public DateTime StatusChangeDate { get; set; }
        public int PalletId { get; set; }
        public int Amount { get; set; } // +1 / -1 в залежності від операції
    }
}
