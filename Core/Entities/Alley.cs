// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Domain.Entities
{
    public class Alley
    {
        public int Id { get; set; }

        public int AlleyIndex { get; set; }
        
        public int Height { get; set; }
        
        public int Length { get; set; }
        
        public int Width { get; set; }
        
        public int CellsPerFloor { get; set; }

        public List<Sector> Sectors { get; set; } = new();
    }

}
