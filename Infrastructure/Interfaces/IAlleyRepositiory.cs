using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAlleyRepositiory : IRepository<Alley>
    {
        public void AddSectorToAlley(int alley_index, Sector sector);

        public void RemoveSectorFromAlley(int alley_index, int sector_index);
    }
}
