using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Sector
    {
        public int alley_index; // індекс алеї в якій знаходиться сектор
        public int sector_index; // індекс сектору в алеї
        public string starting_cell_index; // індекс початкової комірки сектора
        public string ending_cell_index; // індекс кінцевої комірки сектора

        private int floors { get; } // кількість поверхів (рядів)
        public int Floors { get { return floors; } }
    }
}
