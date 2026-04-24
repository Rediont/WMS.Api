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
            
            builder.Property(c=> c.ClientId)
                .IsRequired();

            builder.HasMany(c => c.Documents)
                   .WithOne(document => document.Contract)
                   .HasForeignKey(document => document.ContractId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);
            
        }
    }
}
