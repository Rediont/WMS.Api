using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CellStatusLog
    {
        public long Id { get; set; } // Унікальний ID запису

        public int AlleyId { get; set; }
        public int CellId { get; set; }
        public int ContractId { get; set; }
        public string PalletId { get; set; }
        public DateTime OperationDate { get; set; }
        public int Amount { get; set; }
        public int PalletTypeId { get; set; }
        public virtual PalletTypes PalletType { get; set; }
    }
}
