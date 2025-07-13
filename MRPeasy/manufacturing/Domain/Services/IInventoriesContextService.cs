using MRPeasy.Manufacturing.Domain.Model.ValueObjects;

namespace MRPeasy.Manufacturing.Domain.Services;

/// <summary>
///     Anti-Corruption Layer interface for accessing Inventories bounded context
/// </summary>
public interface IInventoriesContextService
{
    /// <summary>
    ///     Validates if a product exists by its product number
    /// </summary>
    /// <param name="productNumber">Product number to validate</param>
    /// <returns>True if product exists, false otherwise</returns>
    Task<bool> ProductExistsAsync(ItemProductNumber productNumber);

    /// <summary>
    ///     Gets the current production quantity for a product
    /// </summary>
    /// <param name="productNumber">Product number</param>
    /// <returns>Current production quantity</returns>
    Task<int> GetCurrentProductionQuantityAsync(ItemProductNumber productNumber);

    /// <summary>
    ///     Gets the maximum production capacity for a product
    /// </summary>
    /// <param name="productNumber">Product number</param>
    /// <returns>Maximum production capacity</returns>
    Task<int> GetMaxProductionCapacityAsync(ItemProductNumber productNumber);

    /// <summary>
    ///     Updates the current production quantity for a product
    /// </summary>
    /// <param name="productNumber">Product number</param>
    /// <param name="quantityToAdd">Quantity to add to current production</param>
    /// <returns>True if update was successful, false otherwise</returns>
    Task<bool> UpdateCurrentProductionQuantityAsync(ItemProductNumber productNumber, int quantityToAdd);

    /// <summary>
    ///     Validates if adding the required quantity would exceed the maximum production capacity
    /// </summary>
    /// <param name="productNumber">Product number</param>
    /// <param name="requiredQuantity">Required quantity to add</param>
    /// <returns>True if capacity would be exceeded, false otherwise</returns>
    Task<bool> WouldExceedCapacityAsync(ItemProductNumber productNumber, int requiredQuantity);
}