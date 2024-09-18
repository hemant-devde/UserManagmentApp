
using UserManagmentSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace UserManagmentSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}