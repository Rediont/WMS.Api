using Domain.Interface;

namespace Domain.Entities
{
    public class WarehouseSettings : IEntity
    {
        public int Id { get; set; }

        public int NumberOfAlleys { get; set; }

        public int NumberOfAlleyFloors { get; set; }

        public int NumberOfCellsInAlley { get; set; }

        public int NumberOfCellsInAlleyFloor { get; set; }

        public int NumberOfCells { get; set; }

    }
}