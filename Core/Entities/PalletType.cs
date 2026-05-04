using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PalletType : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double RequiredCapacity { get; set; }
    }
}
