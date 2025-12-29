using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.EntityTypeConfigs
{
    public class AlleyEntityTypeConfig : IEntityTypeConfiguration<Alley>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Alley> builder)
        {
            builder.ToTable("Alleys");
            
            builder.HasKey(a => a.AlleyIndex);
            
            builder.Property(a => a.Height).IsRequired();
            
            builder.Property(a => a.Length).IsRequired();
            
            builder.Property(a => a.Width).IsRequired();
            
            builder.Property(a => a.CellsPerFloor).IsRequired();
            
            builder.HasMany(a => a.Sectors)
                   .WithOne()
                   .HasForeignKey("AlleyIndex")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
