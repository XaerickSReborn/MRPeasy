using Microsoft.EntityFrameworkCore;
using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Domain.Model.ValueObjects;
using MRPeasy.Manufacturing.Domain.Model.ValueObjects;
using MRPeasy.Manufacturing.Domain.Services;
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace MRPeasy.Manufacturing.Infrastructure.ACL;

/// <summary>
///     Anti-Corruption Layer implementation for accessing Inventories bounded context
/// </summary>
public class InventoriesContextService : IInventoriesContextService
{
    private readonly AppDbContext _context;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="context">Database context</param>
    public InventoriesContextService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Validates if a product exists by its product number
    /// </summary>
    /// <param name="productNumber">Product number to validate</param>
    /// <returns>True if product exists, false otherwise</returns>
    public async Task<bool> ProductExistsAsync(ItemProductNumber productNumber)
    {
        var productNumberGuid = productNumber.Value;
        return await _context.Set<Product>()
            .AnyAsync(p => p.ProductNumber == ProductNumber.FromValue(productNumberGuid));
    }

    /// <summary>
    ///     Gets the current production quantity for a product
    /// </summary>
    /// <param name="productNumber">Product number</param>
    /// <returns>Current production quantity</returns>
    public async Task<int> GetCurrentProductionQuantityAsync(ItemProductNumber productNumber)
    {
        var productNumberGuid = productNumber.Value;
        var product = await _context.Set<Product>()
            .FirstOrDefaultAsync(p => p.ProductNumber == ProductNumber.FromValue(productNumberGuid));
        
        return product?.CurrentProductionQuantity ?? 0;
    }

    /// <summary>
    ///     Gets the maximum production capacity for a product
    /// </summary>
    /// <param name="productNumber">Product number</param>
    /// <returns>Maximum production capacity</returns>
    public async Task<int> GetMaxProductionCapacityAsync(ItemProductNumber productNumber)
    {
        var productNumberGuid = productNumber.Value;
        var product = await _context.Set<Product>()
            .FirstOrDefaultAsync(p => p.ProductNumber == ProductNumber.FromValue(productNumberGuid));
        
        return product?.MaxProductionCapacity ?? 0;
    }

    /// <summary>
    ///     Updates the current production quantity for a product
    /// </summary>
    /// <param name="productNumber">Product number</param>
    /// <param name="quantityToAdd">Quantity to add to current production</param>
    /// <returns>True if update was successful, false otherwise</returns>
    public async Task<bool> UpdateCurrentProductionQuantityAsync(ItemProductNumber productNumber, int quantityToAdd)
    {
        var productNumberGuid = productNumber.Value;
        var product = await _context.Set<Product>()
            .FirstOrDefaultAsync(p => p.ProductNumber == ProductNumber.FromValue(productNumberGuid));
        
        if (product == null)
            return false;

        try
        {
            // Use the domain method to update production quantity
            var updateResult = product.UpdateCurrentProductionQuantity(quantityToAdd);
            if (!updateResult)
                return false;
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     Validates if adding the required quantity would exceed the maximum production capacity
    /// </summary>
    /// <param name="productNumber">Product number</param>
    /// <param name="requiredQuantity">Required quantity to add</param>
    /// <returns>True if capacity would be exceeded, false otherwise</returns>
    public async Task<bool> WouldExceedCapacityAsync(ItemProductNumber productNumber, int requiredQuantity)
    {
        var productNumberGuid = productNumber.Value;
        var product = await _context.Set<Product>()
            .FirstOrDefaultAsync(p => p.ProductNumber == ProductNumber.FromValue(productNumberGuid));
        
        if (product == null)
            return true; // If product doesn't exist, consider it as exceeding capacity

        var newQuantity = product.CurrentProductionQuantity + requiredQuantity;
        return newQuantity > product.MaxProductionCapacity;
    }
}