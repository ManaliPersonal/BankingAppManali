using Microsoft.EntityFrameworkCore;
using Transaction.API.Models;

namespace Transaction.API.Database
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        {

        }

        public DbSet<AccountTransaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountTransaction>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2); // 18 is total digits, 2 is for decimal places
    }
    }
}