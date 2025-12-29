// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Domain.Entities
{
    public class Cell
    {
        public int AlleyIndex { get; set; } // індекс алеї в якій знаходиться комірка
        public int CellIndex { get; set; } // індекс комірки
        public int FloorIndex { get; set; } // індекс поверху на якому знаходиться комірка

        public decimal totalCapacity = 3.0m;

        public bool isOccupied = false;

        public List<Pallet> StoredPallets = new();

        //public Item item;
    }
}
