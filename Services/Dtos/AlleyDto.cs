using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class AlleyDto
    {
        public int Id { get; set; }
        public int AlleyIndex { get; set; } // індекс алеї в якій знаходиться комірка

        // можливо треба буде переробити на SectorDto
        public List<Sector> Sectors { get; set; }
    }
}
