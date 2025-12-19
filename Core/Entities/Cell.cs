// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Core.Entities
{
    public enum CellStatus
    {
        Full,
        Empty,
        SemiFull,
    }

    public class Cell
    {
        private int alley_index; // індекс алеї в якій знаходиться комірка
        private int cell_index; // індекс комірки
        private bool is_occupied; // чи зайнята комірка

        private int height; // висота комірки

        public CellStatus status;

        public int Alley_index { get { return alley_index; } }
        public int Cell_index { get { return cell_index; } } // індекс комірки
        public bool Is_occupied { get { return is_occupied; } }

        public Item item;

    }
}
