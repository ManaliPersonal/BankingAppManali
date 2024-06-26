
using Account.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Account.API.Database
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {

        }

        public DbSet<BankAccount>? Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BankAccount>()
                .Property(p => p.AccountBalance)
                .HasPrecision(18, 2); 
        }
    }

}