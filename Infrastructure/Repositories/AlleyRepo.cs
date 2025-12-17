using Core.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    internal class AlleyRepo : IAlleyRepositiory
    { 
        private readonly DbContext _dbContext;

        public void AddAlley(Alley alley)
        {
            _dbContext.Alleys.Add(alley);
        }

        public void RemoveAlley(int alley_index)
        {
            Alley? targetAlley = this._dbContext.Alleys.Find(alley_index);

            if (targetAlley == null)
            {
                throw new Exception("Alley not found");
            }

            _dbContext.Alleys.Remove(targetAlley);
            _dbContext.SaveChanges();
        }

        public void AddSectorToAlley(int alley_index, Sector sector)
        {
            Alley? targetAlley = _dbContext.Alleys.Find(alley_index);

            if (targetAlley == null)
            {
                throw new Exception("Alley not found");
            }

            targetAlley.Sectors.Add(sector);
            _dbContext.SaveChanges();
        }

        // видалити сектор з алеї за індексом сектору
        // потрібен тест про відсутність сектору
        public void RemoveSectorFromAlley(int alley_index, int sector_index)
        {
            Alley? targetAlley = _dbContext.Alleys.Find(alley_index);

            if (targetAlley == null)
            {
                throw new Exception("Alley not found");
            }

            targetAlley.Sectors.RemoveAll(s => s.sector_index == sector_index);
            _dbContext.SaveChanges();
        }

        public List<Alley> GetAllAlleys()
        {
            return _dbContext.Alleys.ToList();
        }
    }
}
