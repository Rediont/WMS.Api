using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    internal class AlleyService : IAlleyService
    {
        private readonly IRepository<Alley> _alleyRepository;
        private readonly ISectorService _sectorService;
        public AlleyService(IRepository<Alley> alleyRepository, ISectorService sectorService)
        {
            _alleyRepository = alleyRepository;
            _sectorService = sectorService;
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
            if(length % 3 != 0)
            {
                throw new Exception("Alley length must be multiple of 3");
            }

            Alley newAlley = new Alley
            {
                Height = height,
                Length = length,
                Width = width,
                Sectors = null
            };

            await _alleyRepository.AddAsync(newAlley);
        }

        // потенційно непотрібно
        private void DeleteAlley(int id)
        {
            Alley? alley = _alleyRepository.GetByIdAsync(id).Result;
            if (alley == null)
            {
                throw new Exception("Alley not found");
            }
            _alleyRepository.Delete(alley);
        }

        public void AddSectorToAlley(int alley_index, Sector sector)
        {
            Alley? targetAlley = _alleyRepository.GetByIdAsync(alley_index).Result;

            if (targetAlley == null)
            {
                throw new Exception("Alley not found");
            }
            
            if (sector.alleyIndex != alley_index)
            {
                throw new Exception("Sector alley index does not match the target alley index");
            }

            if (targetAlley.Sectors == null)
            {
                targetAlley.Sectors = new List<Sector>();
            }

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
