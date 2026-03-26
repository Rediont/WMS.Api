using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.LookUpDtos
{
    internal class ContractInfoLookUpDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ContractName { get; set; }
    }
}
