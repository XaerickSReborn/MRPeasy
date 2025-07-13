using MRPeasy.Manufacturing.Domain.Model.Aggregates;
using MRPeasy.Manufacturing.Interfaces.REST.Resources;

namespace MRPeasy.Manufacturing.Interfaces.REST.Transform;

/// <summary>
///     Assembler to transform BillOfMaterialsItem entity to BillOfMaterialsItemResource
/// </summary>
public static class BillOfMaterialsItemResourceFromEntityAssembler
{
    /// <summary>
    ///     Transforms a BillOfMaterialsItem entity to BillOfMaterialsItemResource
    /// </summary>
    /// <param name="entity">Entity to transform</param>
    /// <returns>Resource object</returns>
    public static BillOfMaterialsItemResource ToResourceFromEntity(BillOfMaterialsItem entity)
    {
        return new BillOfMaterialsItemResource
        {
            Id = entity.Id,
            BillOfMaterialsId = entity.BillOfMaterialsId,
            ProductNumber = entity.ItemProductNumber.ToString(),
            BatchId = entity.BatchId,
            RequiredQuantity = entity.RequiredQuantity,
            ScheduledStartAt = entity.ScheduledStartAt,
            RequiredAt = entity.RequiredAt
        };
    }
}