using Microsoft.EntityFrameworkCore;
using MRPeasy.Manufacturing.Domain.Model.Aggregates;
using MRPeasy.Manufacturing.Domain.Model.ValueObjects;
using MRPeasy.Manufacturing.Domain.Repositories;
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Configuration;
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace MRPeasy.Manufacturing.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Repository implementation for Bill of Materials Item aggregate
/// </summary>
public class BillOfMaterialsItemRepository : BaseRepository<BillOfMaterialsItem>, IBillOfMaterialsItemRepository
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="context">Database context</param>
    public BillOfMaterialsItemRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    ///     Checks if a combination of itemProductNumber, batchId and billOfMaterialsId already exists
    /// </summary>
    /// <param name="itemProductNumber">Item product number</param>
    /// <param name="batchId">Batch identifier</param>
    /// <param name="billOfMaterialsId">Bill of Materials identifier</param>
    /// <returns>True if combination exists, false otherwise</returns>
    public async Task<bool> ExistsByCombinationAsync(ItemProductNumber itemProductNumber, int batchId, int billOfMaterialsId)
    {
        var itemProductNumberGuid = itemProductNumber.Value;
        return await Context.Set<BillOfMaterialsItem>()
            .AnyAsync(item => 
                item.ItemProductNumber == ItemProductNumber.FromValue(itemProductNumberGuid) &&
                item.BatchId == batchId &&
                item.BillOfMaterialsId == billOfMaterialsId);
    }

    /// <summary>
    ///     Gets all Bill of Materials Items by Bill of Materials ID
    /// </summary>
    /// <param name="billOfMaterialsId">Bill of Materials identifier</param>
    /// <returns>List of Bill of Materials Items</returns>
    public async Task<IEnumerable<BillOfMaterialsItem>> FindByBillOfMaterialsIdAsync(int billOfMaterialsId)
    {
        return await Context.Set<BillOfMaterialsItem>()
            .Where(item => item.BillOfMaterialsId == billOfMaterialsId)
            .ToListAsync();
    }
}