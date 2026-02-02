using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityTypeConfigs
{
    internal class CellStatusLogEntityTypeConfig : IEntityTypeConfiguration<CellStatusLog>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CellStatusLog> builder)
        {
            builder.HasKey(csl => csl.Id);

            builder.HasIndex(x => x.ContractId); 

            builder.HasIndex(x => x.OperationDate);

            builder.Property(csl => csl.AlleyId)
                .IsRequired();

            builder.Property(csl => csl.CellId)
                .IsRequired();

            builder.Property(csl => csl.ContractId)
                .IsRequired();

            builder.Property(csl => csl.PalletId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(csl => csl.OperationDate)
                .IsRequired();

            builder.Property(csl => csl.Amount)
                .IsRequired();

            builder.Property(csl => csl.PalletTypeId)
                .IsRequired();

            builder.HasOne(csl => csl.PalletType)
                .WithMany()
                .HasForeignKey(csl => csl.PalletTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
