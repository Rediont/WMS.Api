using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Services.Dtos;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class OutboundShipmentService : IOutboundShipmentService
    {
        private readonly IRepository<OutboundShipment> _outboundShipmentRepository;
        private readonly IRepository<Pallet> _palletRepository;
        private readonly IMapper _mapper;

        public OutboundShipmentService(
            IRepository<OutboundShipment> outboundShipmentRepository,
            IRepository<Pallet> palletRepository,
            IMapper mapper)
        {
            _outboundShipmentRepository = outboundShipmentRepository;
            _palletRepository = palletRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OutboundShipmentDto>> GetAllShipmentsAsync()
        {
            var shipments = await _outboundShipmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OutboundShipmentDto>>(shipments);
        }

        public Task<OutboundShipmentDto> GetShipmentByIdAAsync(int id)
        {
            var shipment =  _outboundShipmentRepository.GetByIdAsync(id);
            if (shipment == null)
            {
                throw new KeyNotFoundException($"Shipment with id {id} not found.");
            }
            return _mapper.Map<Task<OutboundShipmentDto>>(shipment);
        }

        public async Task<bool> AddShipment(int contractId, List<int> palletIds)
        {
            var pallets = await _palletRepository.GetByIdsAsync(palletIds);
            
            OutboundShipment newShipment = new OutboundShipment
            {
                ContractId = contractId,
                ShipmentDate = DateTime.Now,
                ShippedPallets = pallets.ToList()
            };
            _outboundShipmentRepository.AddAsync(newShipment);
            return true;
        }

        public async Task<bool> RemoveShipmentByIdAsync(int id)
        {
            var shipment = await  _outboundShipmentRepository.GetByIdAsync(id);
            if (shipment == null)
            {
                return false;
            }
            _outboundShipmentRepository.Delete(shipment);
            return true;
        }
    }
}
