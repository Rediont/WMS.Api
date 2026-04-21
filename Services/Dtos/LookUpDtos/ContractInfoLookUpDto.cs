using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.LookUpDtos
{
    public class ContractInfoLookupDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ContractName { get; set; }
        public int Status { get; set; }
    }
}
