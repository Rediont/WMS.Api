using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            
            builder.Property(s => s.floorIndex)
                .IsRequired();

            builder.Property(s => s.reserveStartDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.Property(s => s.reserveEndDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.HasOne<Sector>()
                   .WithMany()
                   .HasForeignKey(s => s.alleyIndex)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
