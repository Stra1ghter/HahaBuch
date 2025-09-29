using HahaBuch.Category;
using HahaBuch.Transaction;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HahaBuch.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUserEntity, IdentityRole<Guid>, Guid>(options), IDataProtectionKeyContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUserEntity>()
            .HasIndex(u => u.NormalizedEmail)
            .IsUnique();

        builder.Entity<TransactionEntity>()
            .HasIndex(t => t.VaultEntityId);

        builder.Entity<TransactionEntity>()
            .HasIndex(t => t.CategoryEntityId);

        builder.Entity<TransactionEntity>()
            .HasIndex(t => t.DateTime);
    }
    
    public DbSet<VaultEntity> Vaults { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
}