using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class CellStatusDto
    {
        public int Id { get; set; } // Унікальний ID запису

        public int AlleyId { get; set; }
        public int CellId { get; set; }
        public int ContractId { get; set; }
        public string PalletId { get; set; }

        public DateTime OperationDate { get; set; }

        public int Amount { get; set; } // Кількість палет, доданих або видалених (від -3 до 3)

        public int PalletTypeId { get; set; }
    }
}
