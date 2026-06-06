using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ContractDtos
{
    public class NewContractDataDto
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public ContractStatus currentStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
