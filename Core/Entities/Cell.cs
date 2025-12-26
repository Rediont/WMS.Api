// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Domain.Entities
{
    public enum CellStatus
    {
        Full,
        Empty
    }

    public class Cell
    {
        public int alleyIndex; // індекс алеї в якій знаходиться комірка
        public int cellIndex; // індекс комірки
        public int floorIndex; // індекс поверху на якому знаходиться комірка

        public decimal totalCapacity = 3.0m;

        // =====================================================
        // потрібно обдумати спосіб запису зберігання в комірці
        // можливо треба окрему таблицю з датами зберігання
        // =====================================================
        public CellStatus status;

        public List<Pallet> StoredPallets;

        //public Item item;
    }
}
