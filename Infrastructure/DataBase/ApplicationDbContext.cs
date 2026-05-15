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
        public DbSet<WmsDocument> Documents { get; set; }
        public DbSet<WmsDocumentItem> WmsDocumentItems { get; set; }
        public DbSet<Pallet> Pallets { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<PalletType> PalletTypes { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<InventoryBalance> InventoryBalances { get; set; }
        public DbSet<WarehouseSettings> WarehouseOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.HasPostgresEnum<ContractStatus>();
            //modelBuilder.HasPostgresEnum<CellStatus>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            modelBuilder.Entity<WarehouseSettings>()
                .HasData(new WarehouseSettings
                {
                    Id = 1, // Фіксований ID
                    NumberOfAlleys = 10,
                    NumberOfAlleyFloors = 5,
                    NumberOfCellsInAlley = 100,
                    NumberOfCellsInAlleyFloor = 20,
                    NumberOfCells = 5000
                });

        }

    }
}
