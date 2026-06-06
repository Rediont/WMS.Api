using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityTypeConfigs
{
    internal class WmsDocumentItemEntityTypeConfig : IEntityTypeConfiguration<WmsDocumentItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WmsDocumentItem> builder)
        {
            builder.ToTable("WmsDocumentItems");

            builder.HasKey(di => di.Id);

            builder.Property(di => di.ExpectedAmount)
                .IsRequired();

            builder.HasOne(di => di.PalletType)
                .WithMany()
                .HasForeignKey(di => di.PalletTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
