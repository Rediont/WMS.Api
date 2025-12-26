using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntityTypeConfigs
{
    public class ClientEntityTypeConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");
            
            builder.HasKey(c => c.id);
            
            builder.Property(c => c.name)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(c => c.EDRPO)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.email)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(c => c.contactPersonName)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(c => c.contactPersonPhone)
                .IsRequired()
                .HasMaxLength(15);
            
            builder.HasMany(c => c.ContractList)
                .WithOne()
                .HasForeignKey("ClientId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}
