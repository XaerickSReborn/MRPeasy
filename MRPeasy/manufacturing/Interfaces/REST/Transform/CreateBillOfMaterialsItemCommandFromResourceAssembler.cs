using MRPeasy.Manufacturing.Domain.Model.Commands;
using MRPeasy.Manufacturing.Interfaces.REST.Resources;

namespace MRPeasy.Manufacturing.Interfaces.REST.Transform;

/// <summary>
///     Assembler to transform CreateBillOfMaterialsItemResource to CreateBillOfMaterialsItemCommand
/// </summary>
public static class CreateBillOfMaterialsItemCommandFromResourceAssembler
{
    /// <summary>
    ///     Transforms a CreateBillOfMaterialsItemResource to CreateBillOfMaterialsItemCommand
    /// </summary>
    /// <param name="billOfMaterialsId">Bill of Materials ID from route</param>
    /// <param name="resource">Resource to transform</param>
    /// <returns>Command object</returns>
    public static CreateBillOfMaterialsItemCommand ToCommandFromResource(int billOfMaterialsId, CreateBillOfMaterialsItemResource resource)
    {
        return new CreateBillOfMaterialsItemCommand(
            billOfMaterialsId,
            resource.ItemProductNumber,
            resource.BatchId,
            resource.RequiredQuantity,
            resource.ScheduledStartAt,
            resource.RequiredAt);
    }
}