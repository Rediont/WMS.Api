using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityTypeConfigs
{
    internal class SectorEntityTypeConfig : IEntityTypeConfiguration<Sector>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Sector> builder)
        {
            builder.HasKey(s => new { s.alleyIndex, s.sectorIndex });
            
            builder.Property(s => s.startingCellIndex)
                .IsRequired();
            
            builder.Property(s => s.endingCellIndex)
                .IsRequired();
            
            builder.Property(s => s.Floors)
                .IsRequired();

            builder.HasOne<Sector>()
                   .WithMany()
                   .HasForeignKey(s => s.alleyIndex)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
