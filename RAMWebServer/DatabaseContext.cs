using Microsoft.EntityFrameworkCore;
using RAMWebServer.Models;
using System.Security.Cryptography.X509Certificates;

namespace RAMWebServer
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            //Создаем базу данных при первом вызове
            Database.EnsureCreated();
        }
    }
}
