using MRPeasy.Manufacturing.Domain.Model.Aggregates;
using MRPeasy.Manufacturing.Domain.Model.ValueObjects;
using MRPeasy.Shared.Domain.Repositories;

namespace MRPeasy.Manufacturing.Domain.Repositories;

/// <summary>
///     Repository interface for Bill of Materials Item aggregate
/// </summary>
public interface IBillOfMaterialsItemRepository : IBaseRepository<BillOfMaterialsItem>
{
    /// <summary>
    ///     Checks if a combination of itemProductNumber, batchId and billOfMaterialsId already exists
    /// </summary>
    /// <param name="itemProductNumber">Item product number</param>
    /// <param name="batchId">Batch identifier</param>
    /// <param name="billOfMaterialsId">Bill of Materials identifier</param>
    /// <returns>True if combination exists, false otherwise</returns>
    Task<bool> ExistsByCombinationAsync(ItemProductNumber itemProductNumber, int batchId, int billOfMaterialsId);

    /// <summary>
    ///     Gets all Bill of Materials Items by Bill of Materials ID
    /// </summary>
    /// <param name="billOfMaterialsId">Bill of Materials identifier</param>
    /// <returns>List of Bill of Materials Items</returns>
    Task<IEnumerable<BillOfMaterialsItem>> FindByBillOfMaterialsIdAsync(int billOfMaterialsId);
}