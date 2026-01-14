using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos
{
    public class CellDto
    {
        public int Id { get; set; }

        public int AlleyIndex { get; set; }

        public int CellIndex { get; set; }

        public int FloorIndex { get; set; }

        public bool IsOccupied { get; set; }

        public List<Pallet> StoredPallets = new();
    }
}
