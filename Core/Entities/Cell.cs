// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Domain.Entities
{
    public enum CellStatus
    {
        Full,
        Empty,
        SemiFull,
    }

    public class Cell
    {
        private int alleyIndex; // індекс алеї в якій знаходиться комірка
        private int cellIndex; // індекс комірки
        public bool isOccupied; // чи зайнята комірка

        public int height; // висота комірки

        public CellStatus status;

        public int AlleyIndex { get { return alleyIndex; } }
        public int CellIndex { get { return cellIndex; } } // індекс комірки
        public bool IsOccupied { get { return isOccupied; } }

        public Item item;

    }
}
