using Dale.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dale.Api.Data;

public sealed class DaleDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public DaleDbContext(DbContextOptions<DaleDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DaleDbContext).Assembly);
    }
}