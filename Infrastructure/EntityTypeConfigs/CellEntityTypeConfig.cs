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
                .HasDefaultValue(3.0m);

            builder.HasMany(c => c.StoredPallets) 
                .WithOne(p => p.cell)         
                .HasForeignKey(p => new { p.alleyId, p.cellId })
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(c => c.item)
            //       .WithOne()
            //       .HasForeignKey<Item>("CellAlleyIndex", "CellIndex")
            //       .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
