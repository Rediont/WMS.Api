using Services.Dtos.LookUpDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.LookupDtos
{
    public class GlobalLookupDto
    {
        public IEnumerable<ClientLookupDto> Clients { get; set; }
        public IEnumerable<ContractInfoLookupDto> Contracts { get; set; }
        public IEnumerable<PalletTypeLookupDto> PalletTypes { get; set; }
        public WarehouseSettingsLookupDto WarehouseSettings { get; set; }
        public IEnumerable<DocumentTypeLookupDto> DocumentTypes { get; set; }
    }
}
