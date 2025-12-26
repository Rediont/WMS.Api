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
            builder.HasKey(p => p.id);

            // Зв'язок з типом (обов'язково, щоб знати розмір 1.0/1.5)
            builder.HasOne(p => p.palletType)
                   .WithMany()
                   .HasForeignKey(p => p.palletTypeId);

            // Зв'язок з коміркою (використовуємо складений ключ)
            builder.HasOne(p => p.cell)
                   .WithMany(c => c.StoredPallets) // У комірки є список палет
                   .HasForeignKey(p => new { p.alleyId, p.cellId })
                   .IsRequired(false); // Палета може бути "в дорозі" (без комірки)
        }
    }
}
