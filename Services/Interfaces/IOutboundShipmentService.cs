using Domain.Entities;
using Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOutboundShipmentService
    {
        public Task<IEnumerable<OutboundShipmentDto>> GetAllShipmentsAsync();

        public Task<OutboundShipmentDto> GetShipmentByIdAAsync(int id);

        public Task<bool> AddShipment(int contractId, List<int> palletIds);

        public Task<bool> RemoveShipmentByIdAsync(int id);
    }
}
