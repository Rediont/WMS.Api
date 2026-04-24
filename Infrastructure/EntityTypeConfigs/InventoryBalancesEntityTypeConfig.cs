using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityTypeConfigs
{
    public class InventoryBalancesEntityTypeConfig : IEntityTypeConfiguration<InventoryBalance>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<InventoryBalance> builder)
        {
            builder.ToTable("InventoryBalance");
            builder.HasKey(ib => ib.Id);

            builder.Property(ib => ib.Quantity)
                .IsRequired();

            builder.HasIndex(ib => new { ib.ClientId, ib.ContractId, ib.PalletTypeId })
                .IsUnique();

            builder.HasOne(ib => ib.Client)
                .WithMany()
                .HasForeignKey(ib => ib.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ib => ib.Contract)
                .WithMany()
                .HasForeignKey(ib => ib.ContractId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ib => ib.PalletType)
                .WithMany()
                .HasForeignKey(ib => ib.PalletTypeId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}