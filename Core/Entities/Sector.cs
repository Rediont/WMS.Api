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
        public virtual Alley Alley { get; set; }
        
        public int SectorIndex { get; set; } // індекс сектору в алеї
        
        public int FloorIndex { get; set; } // індекс поверху на якому знаходиться сектор
        
        public int StartingCellIndex { get; set; } // індекс початкової комірки сектора
        public virtual Cell StartingCell { get; set; }
        
        public int EndingCellIndex { get; set; } // індекс кінцевої комірки сектора
        public virtual Cell EndingCell { get; set; }
        
        public int ContractId { get; set; } // ID контракту який резервує сектор
        public virtual Contract Contract { get; set; }
        
        public DateTime ReserveStartDate { get; set; } // дата початку резервування сектора
        
        public DateTime ReserveEndDate { get; set; }
    }
}
