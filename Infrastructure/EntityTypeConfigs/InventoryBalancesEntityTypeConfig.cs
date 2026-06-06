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
    public class InventoryBalancesEntityTypeConfig : IEntityTypeConfiguration<InventoryBalance>
    {
        public void Configure(EntityTypeBuilder<InventoryBalance> builder)
        {
            builder.ToTable("InventoryBalances");

            builder.HasKey(ib => ib.Id);

            builder.Property(ib => ib.Amount)
                .IsRequired();

            builder.Property(ib => ib.TransactionDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.HasOne(ib => ib.Document)
                .WithMany()
                .HasForeignKey(ib => ib.DocumentId)
                .OnDelete(DeleteBehavior.Restrict);

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

            builder.HasOne(ib => ib.BatchDocument)
                .WithMany()
                .HasForeignKey(ib => ib.BatchDocumentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}