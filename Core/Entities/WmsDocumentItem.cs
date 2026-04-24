using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WmsDocumentItem : IEntity
    {
        public int Id { get; set; }

        public int WmsDocumentId { get; set; }
        public virtual WmsDocument Document { get; set; }

        public int PalletTypeId { get; set; }
        public virtual PalletType PalletType { get; set; }

        public int ExpectedAmount { get; set; }

    }
}
