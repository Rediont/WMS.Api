using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.EntityTypeConfigs
{
    internal class WmsDocumentEntityTypeConfig : IEntityTypeConfiguration<WmsDocument>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<WmsDocument> builder)
        {
            builder.ToTable("WmsDocument");
            builder.HasKey(cd => cd.Id);

            builder.Property(cd => cd.CreationDate)
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.HasOne<Contract>()
                .WithMany(c => c.Documents)
                .HasForeignKey(cd => cd.ContractId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cd => cd.Items)
                .WithOne(id => id.Document)
                .HasForeignKey(id => id.WmsDocumentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cd => cd.Pallets)
                .WithOne()
                .HasForeignKey(p => p.WmsDocumentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
