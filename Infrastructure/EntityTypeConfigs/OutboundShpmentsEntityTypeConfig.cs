using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.EntityTypeConfigs
{
    internal class OutboundShpmentsEntityTypeConfig : IEntityTypeConfiguration<OutboundShipment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OutboundShipment> builder)
        {
            builder.ToTable("ContractShipments");
            builder.HasKey(os => os.Id);
            
            builder.Property(os => os.ShipmentDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            
            builder.HasOne<Contract>()
                   .WithMany(c => c.Outbounds)
                   .HasForeignKey(os => os.ContractId)
                   .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasMany(os => os.ShippedPallets)
                   .WithOne()
                   .HasForeignKey("ShipmentId")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
