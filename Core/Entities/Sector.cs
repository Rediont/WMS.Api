using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Sector
    {
        public int alleyIndex; // індекс алеї в якій знаходиться сектор
        public int sectorIndex; // індекс сектору в алеї
        public int floorIndex; // індекс поверху на якому знаходиться сектор
        public int startingCellIndex; // індекс початкової комірки сектора
        public int endingCellIndex; // індекс кінцевої комірки сектора
        public DateTime reserveStartDate; // дата початку резервування сектора
        public DateTime reserveEndDate;
    }
}
