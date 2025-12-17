using Core.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAlleyRepositiory
    {
        public void AddAlley(Alley alley);

        public void RemoveAlley(int alley_index);

        public void AddSectorToAlley(int alley_index, Sector sector);

        public void RemoveSectorFromAlley(int alley_index, int sector_index);

        public List<Alley> GetAllAlleys();

    }
}
