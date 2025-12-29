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
            builder.ToTable("PalletTypes");
            builder.HasKey(pt => pt.TypeId);

            builder.Property(pt => pt.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(pt => pt.RequiredCapacity)
                   .IsRequired();

        }
    }
}
