using Domain.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class AlleyService
    {
        private readonly IRepository<Alley> _alleyRepository;
        public AlleyService(IRepository<Alley> alleyRepository)
        {
            _alleyRepository = alleyRepository;
        }

        public async Task<IEnumerable<Alley>> GetAllAlleys()
        {
            return await _alleyRepository.GetAllAsync();
        }

        public async Task<Alley> GetAlleyByIdAAsync(int id)
        {
            Alley? alley = await _alleyRepository.GetByIdAsync(id);
            if(alley == null)
            {
                throw new Exception("Alley not found");
            }
            return alley;
        }

        public async void AddAlley(int height, int length, int width)
        {

        }
 
        public void AddSectorToAlley(int alley_index, int sectorStartingCellId, int sectorEndingCellId)
        {
            Alley? targetAlley = _alleyRepository.GetByIdAsync(alley_index).Result;

            if (targetAlley == null)
            {
                throw new Exception("Alley not found");
            }

            Sector sector = new Sector
            {
                sectorIndex = targetAlley.Sectors.Count > 0 ? targetAlley.Sectors.Max(s => s.sectorIndex) + 1 : 0,
                startingCellIndex = sectorStartingCellId,
                endingCellIndex = sectorEndingCellId
            };

            targetAlley.Sectors.Add(sector);
            _alleyRepository.Update(targetAlley);
        }

        // видалити сектор з алеї за індексом сектору
        // потрібен тест про відсутність сектору
        public void RemoveSectorFromAlley(int alley_index, int sector_index)
        {
            Alley? targetAlley = _alleyRepository.GetByIdAsync(alley_index).Result;

            if (targetAlley == null)
            {
                throw new Exception("Alley not found");
            }

            if (!targetAlley.Sectors.Any(s => s.sectorIndex == sector_index))
            {
                throw new Exception("Sector not found in the specified alley");
            }

            targetAlley.Sectors.RemoveAll(s => s.sectorIndex == sector_index);
            _alleyRepository.Update(targetAlley);
        }

    }
}
