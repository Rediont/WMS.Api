using AutoMapper;
using Domain.Entities;
using global::Services.Dtos;
using Services.Dtos.CellDtos;
using Services.Dtos.ClientDtos;
using Services.Dtos.ContractDtos;
using Services.Dtos.LookupDtos;
using Services.Dtos.LookUpDtos;
using Services.Dtos.PalletDtos;
using Services.Dtos.WmsDocumentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            CreateMap<PalletType, PalletTypeInfoDto>();

            CreateMap<Client, ClientInfoDto>().ReverseMap();

            CreateMap<ClientCreationDto, Client>();

            CreateMap<Sector, SectorInfoDto>().ReverseMap();

            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.ContractId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Name));
            
            CreateMap<Contract, ContractInfoLookupDto>()
            .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.Name : "Невідомий клієнт"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.CurrentStatus));

            CreateMap<ClientContractCost, ClientContractCostDto>();

            CreateMap<WmsDocument, WmsDocumentInfoDto>()
                // Я припускаю, що у сутності поле називається CreatedDate, а в DTO - CreationDate
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))

                .ForMember(dest => dest.DocumentTypeId, opt => opt.MapFrom(src => (int)src.DocumentType))

                .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src =>
                    $"{(src.DocumentType == DocumentType.InboundReceipt ? "Прихід" : "Відправлення")} №{src.Id}"))

                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<WmsDocumentItem, WmsDocumentItemDto>()
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.WmsDocumentId));


            CreateMap<WarehouseSettings, WarehouseSettingsLookupDto>();

        }
    }
}
