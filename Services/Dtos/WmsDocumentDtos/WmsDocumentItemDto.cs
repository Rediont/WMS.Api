using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WmsDocumentDtos
{
    public class WmsDocumentItemDto
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public int ExpectedAmount { get; set; }
        public int PalletTypeId { get; set; }
    }
}
