using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Domain.Model.ValueObjects;
using MRPeasy.Inventories.Domain.Repositories;
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Configuration;
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MRPeasy.Inventories.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Product usando Entity Framework Core
/// </summary>
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    /// <summary>
    /// Constructor del repositorio de productos
    /// </summary>
    /// <param name="context">Contexto de base de datos</param>
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<Product?> FindByProductNumberAsync(ProductNumber productNumber)
    {
        return await Context.Set<Product>()
            .FirstOrDefaultAsync(p => p.ProductNumber == productNumber.Value);
    }

    /// <inheritdoc />
    public async Task<Product?> FindByNameAsync(string name)
    {
        return await Context.Set<Product>()
            .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower().Trim());
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByProductNumberAsync(ProductNumber productNumber)
    {
        return await Context.Set<Product>()
            .AnyAsync(p => p.ProductNumber == productNumber.Value);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await Context.Set<Product>()
            .AnyAsync(p => p.Name.ToLower() == name.ToLower().Trim());
    }


}