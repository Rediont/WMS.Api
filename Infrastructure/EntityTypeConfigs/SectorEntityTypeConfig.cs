using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityTypeConfigs
{
    internal class SectorEntityTypeConfig : IEntityTypeConfiguration<Sector>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Sector> builder)
        {
            builder.HasKey(s => new { s.AlleyIndex, s.SectorIndex });
            
            builder.Property(s => s.StartingCellIndex)
                .IsRequired();
            
            builder.Property(s => s.EndingCellIndex)
                .IsRequired();
            
            builder.Property(s => s.FloorIndex)
                .IsRequired();

            builder.Property(s => s.ReserveStartDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.Property(s => s.ReserveEndDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.HasOne<Sector>()
                   .WithMany()
                   .HasForeignKey(s => s.AlleyIndex)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
