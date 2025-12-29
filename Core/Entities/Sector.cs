using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sector
    {
        public int AlleyIndex { get; set; } // індекс алеї в якій знаходиться сектор
        public int SectorIndex { get; set; } // індекс сектору в алеї
        public int FloorIndex { get; set; } // індекс поверху на якому знаходиться сектор
        public int StartingCellIndex { get; set; } // індекс початкової комірки сектора
        public int EndingCellIndex { get; set; } // індекс кінцевої комірки сектора
        public int ContractId { get; set; } // ID контракту який резервує сектор
        public DateTime ReserveStartDate { get; set; } // дата початку резервування сектора
        public DateTime ReserveEndDate { get; set; }
    }
}
