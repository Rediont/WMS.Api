using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Dtos.ClientDtos
{
    public class ClientCreationDto
    {
        public string Name { get; set; }
        public string EDRPO { get; set; }
        public string Email { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
    }
}
