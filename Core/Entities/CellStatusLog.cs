using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CellStatusLog : IEntity
    {
        public int Id { get; set; } // Унікальний ID запису

        public int AlleyId { get; set; }
        public virtual Alley Alley { get; set; }

        public int CellId { get; set; }
        public virtual Cell Cell { get; set; }
        
        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        public string PalletId { get; set; }
        public virtual Pallet Pallet { get; set; }

        public DateTime OperationDate { get; set; }
        
        public int Amount { get; set; }
        
        public int PalletTypeId { get; set; }
        public virtual PalletTypes PalletType { get; set; }
    }
}
