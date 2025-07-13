using System.ComponentModel.DataAnnotations;
using MRPeasy.Manufacturing.Domain.Model.ValueObjects;
using MRPeasy.Shared.Domain.Model;

namespace MRPeasy.Manufacturing.Domain.Model.Aggregates;

/// <summary>
///     Bill of Materials Item aggregate root
/// </summary>
public class BillOfMaterialsItem : AuditableAggregateRoot
{
    /// <summary>
    ///     Primary key identifier
    /// </summary>
    [Key]
    public int Id { get; private set; }

    /// <summary>
    ///     Bill of Materials identifier
    /// </summary>
    [Required]
    public int BillOfMaterialsId { get; private set; }

    /// <summary>
    ///     Item product number as owned type
    /// </summary>
    [Required]
    public ItemProductNumber ItemProductNumber { get; private set; } = null!;

    /// <summary>
    ///     Batch identifier
    /// </summary>
    [Required]
    public int BatchId { get; private set; }

    /// <summary>
    ///     Required quantity
    /// </summary>
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Required quantity must be greater than 0")]
    public int RequiredQuantity { get; private set; }

    /// <summary>
    ///     Scheduled start date and time
    /// </summary>
    [Required]
    public DateTime ScheduledStartAt { get; private set; }

    /// <summary>
    ///     Required date and time
    /// </summary>
    [Required]
    public DateTime RequiredAt { get; private set; }



    /// <summary>
    ///     Private constructor for Entity Framework
    /// </summary>
    private BillOfMaterialsItem() : base()
    {
    }

    /// <summary>
    ///     Constructor to create a new Bill of Materials Item
    /// </summary>
    /// <param name="billOfMaterialsId">Bill of Materials identifier</param>
    /// <param name="itemProductNumber">Item product number</param>
    /// <param name="batchId">Batch identifier</param>
    /// <param name="requiredQuantity">Required quantity</param>
    /// <param name="scheduledStartAt">Scheduled start date and time</param>
    /// <param name="requiredAt">Required date and time</param>
    public BillOfMaterialsItem(int billOfMaterialsId, ItemProductNumber itemProductNumber, int batchId, 
        int requiredQuantity, DateTime scheduledStartAt, DateTime requiredAt) : base()
    {
        ValidateBusinessRules(billOfMaterialsId, itemProductNumber, batchId, requiredQuantity, scheduledStartAt, requiredAt);
        
        BillOfMaterialsId = billOfMaterialsId;
        ItemProductNumber = itemProductNumber;
        BatchId = batchId;
        RequiredQuantity = requiredQuantity;
        ScheduledStartAt = scheduledStartAt;
        RequiredAt = requiredAt;
    }

    /// <summary>
    ///     Validates business rules for Bill of Materials Item
    /// </summary>
    /// <param name="billOfMaterialsId">Bill of Materials identifier</param>
    /// <param name="itemProductNumber">Item product number</param>
    /// <param name="batchId">Batch identifier</param>
    /// <param name="requiredQuantity">Required quantity</param>
    /// <param name="scheduledStartAt">Scheduled start date and time</param>
    /// <param name="requiredAt">Required date and time</param>
    /// <exception cref="ArgumentException">Thrown when business rules are violated</exception>
    private static void ValidateBusinessRules(int billOfMaterialsId, ItemProductNumber itemProductNumber, 
        int batchId, int requiredQuantity, DateTime scheduledStartAt, DateTime requiredAt)
    {
        if (billOfMaterialsId <= 0)
            throw new ArgumentException("Bill of Materials ID must be greater than 0", nameof(billOfMaterialsId));
        
        if (itemProductNumber == null)
            throw new ArgumentException("Item product number is required", nameof(itemProductNumber));
        
        if (batchId <= 0)
            throw new ArgumentException("Batch ID must be greater than 0", nameof(batchId));
        
        if (requiredQuantity <= 0)
            throw new ArgumentException("Required quantity must be greater than 0", nameof(requiredQuantity));
        
        // RequiredAt cannot be greater than current date
        if (requiredAt > DateTime.UtcNow)
            throw new ArgumentException("Required date cannot be in the future", nameof(requiredAt));
        
        // ScheduledStartAt must be at least 30 days greater than RequiredAt
        var minimumScheduledStart = requiredAt.AddDays(30);
        if (scheduledStartAt < minimumScheduledStart)
            throw new ArgumentException($"Scheduled start date must be at least 30 days after required date ({minimumScheduledStart:yyyy-MM-dd})", nameof(scheduledStartAt));
    }

    /// <summary>
    ///     Gets the unique combination key for validation
    /// </summary>
    /// <returns>Combination key as string</returns>
    public string GetCombinationKey()
    {
        return $"{ItemProductNumber.Value}_{BatchId}_{BillOfMaterialsId}";
    }
}