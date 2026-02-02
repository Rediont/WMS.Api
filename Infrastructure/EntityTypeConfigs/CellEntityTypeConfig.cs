using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityTypeConfigs
{
    internal class CellEntityTypeConfig : IEntityTypeConfiguration<Cell>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cell> builder)
        {
            builder.ToTable("Cells");
            
            builder.HasKey(c => new { c.AlleyIndex, c.CellIndex });

            builder.Property(c => c.FloorIndex)
                .IsRequired();

            builder.Property(c => c.isOccupied)
                .HasDefaultValue(false);

            builder.Property(c => c.totalCapacity)
                .IsRequired()
                .HasDefaultValue(3);

            builder.Property(c => c.usedCapacity)
                .IsRequired()
                .HasDefaultValue(0);

            builder.HasMany(c => c.StoredPallets) 
                .WithOne(p => p.Cell)         
                .HasForeignKey(p => new { p.AlleyId, p.CellId })
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(c => c.item)
            //       .WithOne()
            //       .HasForeignKey<Item>("CellAlleyIndex", "CellIndex")
            //       .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
