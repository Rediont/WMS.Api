using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.EntityTypeConfigs
{
    internal class InboundReceiptsEntityTypeConfig : IEntityTypeConfiguration<InboundReceipt>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<InboundReceipt> builder)
        {
            builder.ToTable("ContractReceipts");
            builder.HasKey(cd => cd.id);

            builder.Property(cd => cd.receiptDate)
                .HasColumnType("timestamp with time zone") // або "timestamp without time zone"
                .IsRequired();

            builder.HasOne<Contract>()
                   .WithMany(c => c.Inbounds)
                   .HasForeignKey(cd => cd.contractId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(cd => cd.recievedItems)
                   .WithOne()
                   .HasForeignKey("DeliveryId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
