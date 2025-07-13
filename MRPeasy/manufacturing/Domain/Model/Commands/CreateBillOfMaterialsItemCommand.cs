namespace MRPeasy.Manufacturing.Domain.Model.Commands;

/// <summary>
///     Command to create a new Bill of Materials Item
/// </summary>
/// <param name="BillOfMaterialsId">Bill of Materials identifier</param>
/// <param name="ItemProductNumber">Item product number as string</param>
/// <param name="BatchId">Batch identifier</param>
/// <param name="RequiredQuantity">Required quantity</param>
/// <param name="ScheduledStartAt">Scheduled start date and time</param>
/// <param name="RequiredAt">Required date and time</param>
public record CreateBillOfMaterialsItemCommand(
    int BillOfMaterialsId,
    string ItemProductNumber,
    int BatchId,
    int RequiredQuantity,
    DateTime ScheduledStartAt,
    DateTime RequiredAt
);