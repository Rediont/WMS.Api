using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.WmsDocumentDtos
{
    public class NewDocumentItemsDto
    {
        // Ключ (int) - це PalletTypeId
        // Значення (int) - це Amount (кількість)
        public Dictionary<int, int> Items { get; set; }
    }
}
