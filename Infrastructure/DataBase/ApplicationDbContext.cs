using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.DataBase
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){ }

        public DbSet<Alley> Alleys { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<InboundReceipt> Receipts { get; set; }
        public DbSet<OutboundShipment> Shipments { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresEnum<ContractStatus>();
            //modelBuilder.HasPostgresEnum<CellStatus>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Це скаже Postgres генерувати UUID автоматично при додаванні нового запису
        //    modelBuilder.Entity<Client>()
        //        .Property(c => c.id)
        //        .HasDefaultValueSql("gen_random_uuid()");
        //}
    }
}
