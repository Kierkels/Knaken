using Kierkels.Knaken.Domain.Entities;
using Kierkels.Knaken.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Kierkels.Knaken.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Define your DbSets for entities
    public DbSet<TransactionEntity> Transactions { get; set; }

    // Configure EF Core settings (if needed)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Additional configurations, like applying Fluent API configurations.
        modelBuilder.ApplyConfiguration(new TransactionEntityConfiguration());
    }
}
