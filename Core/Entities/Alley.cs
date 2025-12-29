// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Domain.Entities
{
    public class Alley
    {
        public int AlleyIndex { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int CellsPerFloor { get; set; }

        // Без virtual — використовуємо Explicit/Eager Loading
        public List<Sector> Sectors { get; set; } = new();
    }

}
