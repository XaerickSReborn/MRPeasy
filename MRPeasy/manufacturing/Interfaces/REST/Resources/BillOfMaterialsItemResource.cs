namespace MRPeasy.Manufacturing.Interfaces.REST.Resources;

/// <summary>
///     Resource representing a Bill of Materials Item in responses
/// </summary>
public record BillOfMaterialsItemResource
{
    /// <summary>
    ///     Unique identifier
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    ///     Bill of Materials identifier
    /// </summary>
    public int BillOfMaterialsId { get; init; }

    /// <summary>
    ///     Product number identifier for the item
    /// </summary>
    public string ProductNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Batch identifier
    /// </summary>
    public int BatchId { get; init; }

    /// <summary>
    ///     Required quantity for production
    /// </summary>
    public int RequiredQuantity { get; init; }

    /// <summary>
    ///     Scheduled start date and time
    /// </summary>
    public DateTime ScheduledStartAt { get; init; }

    /// <summary>
    ///     Required date and time
    /// </summary>
    public DateTime RequiredAt { get; init; }
}