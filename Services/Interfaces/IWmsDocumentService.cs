using Domain.Entities;
using Services.Dtos.LookupDtos;
using Services.Dtos.WmsDocumentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWmsDocumentService
    {
        public Task<IEnumerable<WmsDocumentInfoDto>> GetAllDocumentsAsync(int? page);

        public Task<WmsDocumentInfoDto> GetDocumentByIdAsync(int id);

        public Task<List<WmsDocumentInfoDto>> GetDocumentsByIdsAsync(List<int> ids);

        public Task<WmsDocument> CreateReceiptAsync(int contractId, int clientId, DateTime date, NewDocumentItemsDto newItems);

        public Task<WmsDocument> CreateShipmentAsync(int clientId, int contractId, DateTime date, List<Pallet> pallets);

        public Task<WmsDocument> CreateShipmentAsync(int clientId, int contractId, DateTime date, NewDocumentItemsDto newItems);

        public Task<WmsDocument> UpdateDocument(int id, NewDocumentItemsDto newItems);

        public Task<IEnumerable<DocumentTypeLookupDto>> GetDocumentTypesAsync();


    }
}
