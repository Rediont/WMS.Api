using AutoMapper;
using Domain.Entities;
using global::Services.Dtos;
using Services.Dtos.Alley;
using Services.Dtos.CellDtos;
using Services.Dtos.ClientDtos;
using Services.Dtos.ContractDtos;
using Services.Dtos.LookupDtos;
using Services.Dtos.LookUpDtos;
using Services.Dtos.PalletDtos;
using Services.Dtos.PaymentDto;
using Services.Dtos.WarehouseRemains;
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

            CreateMap<Pallet, PalletInfoDto>()
                .ForMember(dest => dest.PalletTypeId, opt => opt.MapFrom(src => src.PalletTypeId))
                .ForMember(dest => dest.PalletTypeName, opt => opt.MapFrom(src => src.PalletType.Name))
                .ForMember(dest => dest.ArrivalDocumentId, opt => opt.MapFrom(src => src.ArrivalDocumentId))
                .ForMember(dest => dest.ArrivalDate, opt => opt.MapFrom(src => src.ArrivalDocument.CreationDate))
                .ForMember(dest => dest.CellIndex, opt => opt.MapFrom(src => src.CellIndex))
                .ForMember(dest => dest.AlleyIndex, opt => opt.MapFrom(src => src.AlleyIndex))
                .ForMember(dest => dest.PalletStatus, opt => opt.MapFrom(src => src.PalletStatus));

            CreateMap<PalletInfoDto, Pallet>();

            CreateMap<PalletType, PalletTypeInfoDto>()
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.RequiredCapacity));

            CreateMap<PalletType, PalletTypeLookupDto>()
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.RequiredCapacity))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost));

            CreateMap<PalletTypeCreationDto, PalletType>()
                .ForMember(dest => dest.RequiredCapacity, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<PalletTypeUpdateDto, PalletType>();

            CreateMap<Client, ClientInfoDto>().ReverseMap();

            CreateMap<ClientCreationDto, Client>();

            CreateMap<Sector, SectorInfoDto>().ReverseMap();

            CreateMap<Contract, ContractDto>()
                .ForMember(dest => dest.ContractId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Client.Id))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))            
                .ForMember(dest => dest.CurrentStatus, opt => opt.MapFrom(src => (int)src.CurrentStatus));

            CreateMap<Contract, ContractInfoLookupDto>()
            .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.Name : "Невідомий клієнт"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.CurrentStatus));

            CreateMap<ClientContractCost, ClientContractCostDto>();

            CreateMap<WmsDocument, WmsDocumentInfoDto>()
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.DocumentTypeId, opt => opt.MapFrom(src => (int)src.DocumentType))
                .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src =>
                    $"{(src.DocumentType == DocumentType.InboundReceipt ? "Прихід" : "Відправлення")} №{src.Id}"))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Contract.Client.Name))
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Contract.Name));

            CreateMap<WmsDocumentItem, WmsDocumentItemDto>()
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.WmsDocumentId));


            CreateMap<WarehouseSettings, WarehouseSettingsLookupDto>();

            CreateMap<InventoryBalance, WarehouseRemainsDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Contract.Name))
                .ForMember(dest => dest.PalletTypeName, opt => opt.MapFrom(src => src.PalletType.Name))
                .ForMember(dest => dest.DocumentName, opt => opt.MapFrom(src =>
                        $"{(src.Document.DocumentType == DocumentType.InboundReceipt ? "Прихід" : "Відправлення")} №{src.Document.Id}"))
                .ForMember(dest => dest.BatchDocumentName, opt => opt.MapFrom(src =>
                        $"{(src.BatchDocument.DocumentType == DocumentType.InboundReceipt ? "Прихід" : "Відправлення")} №{src.BatchDocument.Id}"));

            CreateMap<Bill, BillDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.BillItems))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Contract.Name));

            CreateMap<BillItem, BillItemDto>()
                .ForMember(dest => dest.CostPerDay, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalPrice));

            CreateMap<WmsDocumentItem, DocumentDetailsItemDto>()
                .ForMember(dest => dest.PalletTypeId, opt => opt.MapFrom(src => src.PalletTypeId))
                .ForMember(dest => dest.ExpectedAmount, opt => opt.MapFrom(src => src.ExpectedAmount))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
