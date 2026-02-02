using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityTypeConfigs
{
    internal class ContractEntityTypeConfig : IEntityTypeConfiguration<Contract>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contracts");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(c => c.StartDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            
            builder.Property(c => c.ExpirationDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            
            builder.Property(c => c.CurrentStatus)
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
