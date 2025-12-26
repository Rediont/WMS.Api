// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Domain.Entities
{
    public class Alley
    {
        public int alleyIndex { get; } // для визначення алеї
        public int height; // висота алеї 
        public int length; // довжина алеї
        public int width; // ширина алеї
        public int cellsPerFloor { get; } // кількість комірок на поверх 

        public List<Sector>? Sectors; // список секторів алеї

    }

}
