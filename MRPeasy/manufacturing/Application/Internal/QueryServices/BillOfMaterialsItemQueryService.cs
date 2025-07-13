using MRPeasy.Manufacturing.Domain.Model.Aggregates;
using MRPeasy.Manufacturing.Domain.Model.Queries;
using MRPeasy.Manufacturing.Domain.Repositories;

namespace MRPeasy.Manufacturing.Application.Internal.QueryServices;

/// <summary>
///     Query service for Bill of Materials Item operations
/// </summary>
public class BillOfMaterialsItemQueryService
{
    private readonly IBillOfMaterialsItemRepository _billOfMaterialsItemRepository;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="billOfMaterialsItemRepository">Bill of Materials Item repository</param>
    public BillOfMaterialsItemQueryService(IBillOfMaterialsItemRepository billOfMaterialsItemRepository)
    {
        _billOfMaterialsItemRepository = billOfMaterialsItemRepository;
    }

    /// <summary>
    ///     Handles getting Bill of Materials Items by Bill of Materials ID
    /// </summary>
    /// <param name="query">Query with Bill of Materials ID</param>
    /// <returns>List of Bill of Materials Items</returns>
    public async Task<IEnumerable<BillOfMaterialsItem>> Handle(GetBillOfMaterialsItemsByBillOfMaterialsIdQuery query)
    {
        return await _billOfMaterialsItemRepository.FindByBillOfMaterialsIdAsync(query.BillOfMaterialsId);
    }
}