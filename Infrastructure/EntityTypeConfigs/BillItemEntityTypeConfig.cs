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
    public class BillItemEntityTypeConfig : IEntityTypeConfiguration<BillItem>
    {
        public void Configure(EntityTypeBuilder<BillItem> builder)
        {
            builder.ToTable("BillItems");

            builder.HasKey(bi => bi.Id);

            builder.Property(bi => bi.AmountOfDays)
                .IsRequired();

            builder.Property(bi => bi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(bi => bi.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(bi => bi.PalletType)
                .WithMany()
                .HasForeignKey(bi => bi.PalletTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bi => bi.Bill)
                .WithMany(b => b.BillItems)
                .HasForeignKey(bi => bi.BillId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
