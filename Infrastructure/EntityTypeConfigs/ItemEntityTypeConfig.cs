using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityTypeConfigs
{
    internal class ItemEntityTypeConfig : IEntityTypeConfiguration<Item>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");
            builder.HasKey(i => i.Id);
            
            builder.Property(i => i.name)
                .IsRequired()
                .HasMaxLength(200);
            
            builder.Property(i => i.expirationDate)
                .IsRequired(false);
            
            builder.Property(i => i.boxPerCell)
                .IsRequired();
            
            builder.Property(i => i.unitPerBox)
                .IsRequired(false);

            builder.HasOne<Contract>()
                .WithMany(c => c.itemList)
                .HasForeignKey("contract_id") 
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<InboundReceipt>()
                .WithMany(r => r.recievedItems)
                .HasForeignKey("receipt_id")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<OutboundShipment>()
                .WithMany(s => s.ShippedItems)
                .HasForeignKey("shipment_id")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
