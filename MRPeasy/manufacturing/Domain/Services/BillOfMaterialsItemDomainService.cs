using MRPeasy.Manufacturing.Domain.Model.Aggregates;
using MRPeasy.Manufacturing.Domain.Model.ValueObjects;
using MRPeasy.Manufacturing.Domain.Repositories;

namespace MRPeasy.Manufacturing.Domain.Services;

/// <summary>
///     Domain service for Bill of Materials Item business rules
/// </summary>
public class BillOfMaterialsItemDomainService
{
    private readonly IBillOfMaterialsItemRepository _billOfMaterialsItemRepository;
    private readonly IInventoriesContextService _inventoriesContextService;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="billOfMaterialsItemRepository">Bill of Materials Item repository</param>
    /// <param name="inventoriesContextService">Inventories context service (ACL)</param>
    public BillOfMaterialsItemDomainService(
        IBillOfMaterialsItemRepository billOfMaterialsItemRepository,
        IInventoriesContextService inventoriesContextService)
    {
        _billOfMaterialsItemRepository = billOfMaterialsItemRepository;
        _inventoriesContextService = inventoriesContextService;
    }

    /// <summary>
    ///     Validates if a Bill of Materials Item can be created
    /// </summary>
    /// <param name="billOfMaterialsId">Bill of Materials identifier</param>
    /// <param name="itemProductNumber">Item product number</param>
    /// <param name="batchId">Batch identifier</param>
    /// <param name="requiredQuantity">Required quantity</param>
    /// <returns>Validation result with error message if invalid</returns>
    public async Task<(bool IsValid, string ErrorMessage)> ValidateForCreationAsync(
        int billOfMaterialsId, ItemProductNumber itemProductNumber, int batchId, int requiredQuantity)
    {
        // Check if product exists in inventories context
        var productExists = await _inventoriesContextService.ProductExistsAsync(itemProductNumber);
        if (!productExists)
        {
            return (false, $"Product with number {itemProductNumber} does not exist");
        }

        // Check if combination already exists
        var combinationExists = await _billOfMaterialsItemRepository.ExistsByCombinationAsync(
            itemProductNumber, batchId, billOfMaterialsId);
        if (combinationExists)
        {
            return (false, $"A Bill of Materials Item with the same combination of product number {itemProductNumber}, batch ID {batchId}, and Bill of Materials ID {billOfMaterialsId} already exists");
        }

        // Check if adding the required quantity would exceed the maximum production capacity
        var wouldExceedCapacity = await _inventoriesContextService.WouldExceedCapacityAsync(
            itemProductNumber, requiredQuantity);
        if (wouldExceedCapacity)
        {
            return (false, $"Adding {requiredQuantity} units would exceed the maximum production capacity for product {itemProductNumber}");
        }

        return (true, string.Empty);
    }

    /// <summary>
    ///     Updates the product's current production quantity after creating a Bill of Materials Item
    /// </summary>
    /// <param name="itemProductNumber">Item product number</param>
    /// <param name="requiredQuantity">Required quantity to add</param>
    /// <returns>True if update was successful, false otherwise</returns>
    public async Task<bool> UpdateProductionQuantityAsync(ItemProductNumber itemProductNumber, int requiredQuantity)
    {
        return await _inventoriesContextService.UpdateCurrentProductionQuantityAsync(itemProductNumber, requiredQuantity);
    }
}