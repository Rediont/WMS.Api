using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using global::Services.Dtos;

namespace Services.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Alley, AlleyDto>();

            CreateMap<Cell, CellDto>();

            CreateMap<CellStatusLog, CellStatusDto>();

            CreateMap<Pallet, PalletInfoDto>();

            CreateMap<Client, ClientInfoDto>().ReverseMap();

            CreateMap<Sector, SectorInfoDto>().ReverseMap();

            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.ContractId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Name));

            CreateMap<ClientContractCost, ClientContractCostDto>();

            CreateMap<InboundReceipt, InboundReceiptDto>();
        
            CreateMap<OutboundShipment, OutboundShipmentDto>(); // в процесі
        }
    }
}
