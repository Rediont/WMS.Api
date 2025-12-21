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
            
            builder.Property(c => c.status).IsRequired();
            
            builder.Property(c => c.height).IsRequired();
            
            builder.Property(c => c.isOccupied).IsRequired();
            
            builder.HasOne(c => c.item)
                   .WithOne()
                   .HasForeignKey<Item>("CellAlleyIndex", "CellIndex")
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
