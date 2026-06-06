using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Infrastructure.DataBase // Переконайся, що простір імен збігається з твоїм контекстом
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Вказуємо шлях до папки WMS.Api, де лежить appsettings.json
            // Directory.GetCurrentDirectory() під час міграції буде вказувати на папку Infrastructure
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../WMS.Api");

            // Читаємо конфігурацію
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            // Створюємо опції для контексту
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // ЗАМІНИ "DefaultConnection" на ту назву, яка у тебе в appsettings.json!
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Якщо ти використовуєш PostgreSQL (Npgsql). 
            // Якщо SQL Server - зміни на UseSqlServer
            builder.UseNpgsql(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}