using Domain.Entities;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    internal class AlleyRepo : Repository<Alley> , IRepository<Alley>
    { 
        private readonly DbContext _dbContext;

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
    }
}
