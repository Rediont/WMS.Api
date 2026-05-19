using Services.Dtos.PalletBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IPalletBindingService
    {
        Task BindPalletToCell(int palletId, int cellId, int alleyIndex);

        Task UnbindPalletFromCell(int palletId);

        Task BindMultiple(Dictionary<int, int[]> cellPalletDict, int alleyIndex);

        Task<IEnumerable<ActivePalletsPerDocumentDto>> GetActivePalletsCountPerDocumentAsync();

        Task<IEnumerable<PalletAssignmentDto>> GetUnboundPalletsForDocument(int documentId, int? palletTypeId);
    }
}
