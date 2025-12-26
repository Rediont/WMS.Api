using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityTypeConfigs
{
    internal class ContractEntityTypeConfig : IEntityTypeConfiguration<Contract>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contracts");
            builder.HasKey(c => c.id);

            builder.Property(c => c.name)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(c => c.startDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            
            builder.Property(c => c.expirationDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            
            builder.Property(c => c.currentStatus)
                .IsRequired();
            
            builder.HasMany(c => c.Inbounds)
                   .WithOne()
                   .HasForeignKey("ContractId")
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);
            
            builder.HasMany(c => c.Outbounds)
                   .WithOne()
                   .HasForeignKey("ContractId")
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);

            //builder.HasMany(c => c.itemList)
            //       .WithOne()
            //       .HasForeignKey("ContractId")
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
