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
        public int startingCellIndex; // індекс початкової комірки сектора
        public int endingCellIndex; // індекс кінцевої комірки сектора

        private int floors { get; } // кількість поверхів (рядів)
        public int Floors { get { return floors; } }
    }
}
