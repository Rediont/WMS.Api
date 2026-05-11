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
    internal class PalletEntityTypeConfig : IEntityTypeConfiguration<Pallet>
    {
        public void Configure(EntityTypeBuilder<Pallet> builder)
        {
            builder.ToTable("pallets");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ArrivalDocumentId).IsRequired();

            builder.Property(p => p.PalletStatus).IsRequired();

            builder.HasOne(p => p.PalletType)
                   .WithMany()
                   .HasForeignKey(p => p.PalletTypeId);

            builder.HasOne(p => p.Cell)
                   .WithMany(c => c.StoredPallets) // У комірки є список палет
                   .HasForeignKey(p => new { p.AlleyIndex, p.CellIndex })
                   .IsRequired(false); // Палета може бути "в дорозі" (без комірки)
        }
    }
}
