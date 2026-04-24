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

        public Task<bool> AddInboundReceipt(int contractId, int amount, int palletType, List<int> palletIds);

        public Task<bool> UpdateInboundReceipt(int id, int contractId, int amount, int palletType, List<int> palletIds);

        public Task<bool> AddOutboundShipment(int contractId, int amount, int palletType, List<int> palletIds);

        public Task<bool> UpdateOutboundShipment(int id, int contractId, int amount, int palletType, List<int> palletIds);

        public Task<IEnumerable<DocumentTypeLookupDto>> GetDocumentTypesAsync();
    }
}
