using System.ComponentModel.DataAnnotations;

namespace MRPeasy.Manufacturing.Interfaces.REST.Resources;

/// <summary>
///     Resource for creating a Bill of Materials Item
/// </summary>
public record CreateBillOfMaterialsItemResource
{
    /// <summary>
    ///     Product number identifier for the item
    /// </summary>
    [Required]
    public string ItemProductNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Batch identifier
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "BatchId must be greater than 0")]
    public int BatchId { get; init; }

    /// <summary>
    ///     Required quantity for production
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "RequiredQuantity must be greater than 0")]
    public int RequiredQuantity { get; init; }

    /// <summary>
    ///     Scheduled start date and time
    /// </summary>
    [Required]
    public DateTime ScheduledStartAt { get; init; }

    /// <summary>
    ///     Required date and time
    /// </summary>
    [Required]
    public DateTime RequiredAt { get; init; }
}