// для уніфікації розмірів всі виміри ведуться в сантиматрах

using Domain.Interface;

namespace Domain.Entities
{
    public class Cell : IEntity
    {
        public int Id { get; set; }

        public int AlleyIndex { get; set; } // індекс алеї в якій знаходиться комірка
        public virtual Alley Alley { get; set; }
        
        public int CellIndex { get; set; } // індекс комірки
        
        public int FloorIndex { get; set; }

        public double totalCapacity { get; private set; } = 3;

        public double usedCapacity { get; set; } = 0;

        public bool isOccupied { get; set; } = false;

        public ICollection<Pallet> StoredPallets { get; set; } = new List<Pallet>();

        //public Item item;
    }
}
