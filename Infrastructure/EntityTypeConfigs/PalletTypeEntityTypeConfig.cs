using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityTypeConfigs
{
    internal class PalletTypeEntityTypeConfig : IEntityTypeConfiguration<PalletTypes>
    {
        public void Configure(EntityTypeBuilder<PalletTypes> builder) 
        {

        }
    }
}
