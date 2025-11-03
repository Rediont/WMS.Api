// для уніфікації розмірів всі виміри ведуться в сантиматрах

namespace Core.Entities
{
    public class Alley
    {
        private int alley_index { get; } // для визначення алеї
        public readonly int height; // висота алеї 
        public readonly int length; // довжина алеї
        public readonly int width; // ширина алеї
        private int cells_per_floor { get; } // кількість комірок на поверх 

        public List<Sector> sectors; // список секторів алеї

    }

}
