namespace MRPeasy.Manufacturing.Domain.Model.Queries;

/// <summary>
///     Query to get Bill of Materials Items by Bill of Materials ID
/// </summary>
/// <param name="BillOfMaterialsId">Bill of Materials identifier</param>
public record GetBillOfMaterialsItemsByBillOfMaterialsIdQuery(int BillOfMaterialsId);