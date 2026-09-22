using Microsoft.EntityFrameworkCore;
using Mono.Api.Domain;

namespace Mono.Api.Data;

public sealed class MonoDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public MonoDbContext(DbContextOptions<MonoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MonoDbContext).Assembly);
    }
}