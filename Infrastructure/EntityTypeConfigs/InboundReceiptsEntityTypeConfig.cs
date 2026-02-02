using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.EntityTypeConfigs
{
    internal class InboundReceiptsEntityTypeConfig : IEntityTypeConfiguration<InboundReceipt>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<InboundReceipt> builder)
        {
            builder.ToTable("ContractReceipts");
            builder.HasKey(cd => cd.Id);

            builder.Property(cd => cd.ReceiptDate)
                .HasColumnType("timestamp with time zone") // або "timestamp without time zone"
                .IsRequired();

            builder.HasOne<Contract>()
                .WithMany(c => c.Inbounds)
                .HasForeignKey(cd => cd.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cd => cd.PalletType)
                .WithMany()
                .HasForeignKey(pt => pt.PalletTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(cd => cd.Pallets)
                .WithOne()
                .HasForeignKey(p => p.InboundReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(cd => cd.recievedItems)
            //       .WithOne()
            //       .HasForeignKey("DeliveryId")
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
