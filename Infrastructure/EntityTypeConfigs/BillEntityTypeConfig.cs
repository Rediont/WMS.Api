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
    internal class BillEntityTypeConfig : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.ToTable("Bills");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.TotalCost)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PeriodStartDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.Property(p => p.PeriodEndDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.Property(p => p.PaymentDate)
                .HasColumnType("timestamp with time zone");

            builder.Property(p => p.PaymentMethod)
                .HasMaxLength(50); 

            builder.HasOne(p => p.Contract)
                .WithMany()
                .HasForeignKey(p => p.ContractId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
