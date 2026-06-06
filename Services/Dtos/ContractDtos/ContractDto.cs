using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ContractDtos
{
    public class ContractDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ContractId { get; set; }
        public string ContractName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public int CurrentStatus { get; set; }
    }
}
