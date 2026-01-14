// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Domain.Entities
{
    public class Cell
    {
        public int Id { get; set; }

        public int AlleyIndex { get; set; } // індекс алеї в якій знаходиться комірка
        public virtual Alley Alley { get; set; }
        
        public int CellIndex { get; set; } // індекс комірки
        
        public int FloorIndex { get; set; }

        public decimal totalCapacity = 3.0m;

        public bool isOccupied = false;

        public List<Pallet> StoredPallets = new();

        //public Item item;
    }
}
