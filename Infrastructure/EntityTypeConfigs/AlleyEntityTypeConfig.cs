using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.EntityTypeConfigs
{
    public class AlleyEntityTypeConfig : IEntityTypeConfiguration<Alley>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Alley> builder)
        {
            builder.ToTable("Alleys");
            
            builder.HasKey(a => a.alleyIndex);
            
            builder.Property(a => a.height).IsRequired();
            
            builder.Property(a => a.length).IsRequired();
            
            builder.Property(a => a.width).IsRequired();
            
            builder.Property(a => a.cellsPerFloor).IsRequired();
            
            builder.HasMany(a => a.Sectors)
                   .WithOne()
                   .HasForeignKey("AlleyIndex")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
