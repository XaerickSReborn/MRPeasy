
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Infrastructure.Persistence.EFC.Configuration;
using MRPeasy.Manufacturing.Domain.Model.Aggregates;
using MRPeasy.Manufacturing.Infrastructure.Persistence.EFC.Configuration;
using MRPeasy.Shared.Domain.Model;

namespace MRPeasy.Shared.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
/// Application database context for the Certi Web Platform API.
/// </summary>
/// <param name="options">
///     The options for the database context
/// </param>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// DbSet para la entidad Product del bounded context de inventories
    /// </summary>
    public DbSet<Product> Products { get; set; } = null!;
    
    /// <summary>
    /// DbSet para la entidad BillOfMaterialsItem del bounded context de manufacturing
    /// </summary>
    public DbSet<BillOfMaterialsItem> BillOfMaterialsItems { get; set; } = null!;
   /// <summary>
   ///     On configuring the database context
   /// </summary>
   /// <remarks>
   ///     This method is used to configure the database context.
   ///     It also adds the created and updated date interceptor to the database context.
   /// </remarks>
   /// <param name="builder">
   ///     The option builder for the database context
   /// </param>
   protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

   /// <summary>
   ///     On creating the database model
   /// </summary>
   /// <remarks>
   ///     This method is used to create the database model for the application.
   /// </remarks>
   /// <param name="builder">
   ///     The model builder for the database context
   /// </param>
   protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Aplicar configuraciones de entidades
        builder.ApplyConfiguration(new ProductEntityConfiguration());
        builder.ApplyConfiguration(new BillOfMaterialsItemEntityConfiguration());
        
        builder.UseSnakeCaseNamingConvention();
    }

    /// <summary>
    /// Override SaveChanges to automatically update audit fields
    /// </summary>
    /// <returns>Number of affected rows</returns>
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync to automatically update audit fields
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected rows</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates audit fields for all tracked entities
    /// </summary>
    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<AuditableAggregateRoot>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(AuditableAggregateRoot.CreatedAt)).CurrentValue = DateTime.UtcNow;
            }
            
            if (entry.State == EntityState.Modified)
            {
                entry.Property(nameof(AuditableAggregateRoot.UpdatedAt)).CurrentValue = DateTime.UtcNow;
            }
        }
    }
}