using System.ComponentModel.DataAnnotations;

namespace MRPeasy.Manufacturing.Domain.Model.ValueObjects;

/// <summary>
///     Value Object representing a product number for Bill of Materials Items
/// </summary>
public class ItemProductNumber
{
    /// <summary>
    ///     The unique identifier for the product
    /// </summary>
    public Guid Value { get; private set; }

    /// <summary>
    ///     Private constructor for Entity Framework
    /// </summary>
    private ItemProductNumber()
    {
    }

    /// <summary>
    ///     Constructor to create a new ItemProductNumber from an existing Guid
    /// </summary>
    /// <param name="value">The Guid value</param>
    public ItemProductNumber(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("ItemProductNumber cannot be empty", nameof(value));
        
        Value = value;
    }

    /// <summary>
    ///     Create ItemProductNumber from string representation
    /// </summary>
    /// <param name="value">String representation of the Guid</param>
    /// <returns>ItemProductNumber instance</returns>
    public static ItemProductNumber FromString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ItemProductNumber string cannot be null or empty", nameof(value));
        
        if (!Guid.TryParse(value, out var guid))
            throw new ArgumentException("Invalid ItemProductNumber format", nameof(value));
        
        return new ItemProductNumber(guid);
    }

    /// <summary>
    ///     Creates an ItemProductNumber from a Guid value (for Entity Framework conversion)
    /// </summary>
    /// <param name="value">Guid value to convert</param>
    /// <returns>ItemProductNumber instance</returns>
    public static ItemProductNumber FromValue(Guid value)
    {
        return new ItemProductNumber(value);
    }

    /// <summary>
    ///     Implicit conversion to Guid
    /// </summary>
    /// <param name="itemProductNumber">ItemProductNumber to convert</param>
    /// <returns>Guid value</returns>
    public static implicit operator Guid(ItemProductNumber itemProductNumber)
    {
        return itemProductNumber.Value;
    }

    /// <summary>
    ///     Implicit conversion to string
    /// </summary>
    /// <param name="itemProductNumber">ItemProductNumber to convert</param>
    /// <returns>String representation</returns>
    public static implicit operator string(ItemProductNumber itemProductNumber)
    {
        return itemProductNumber.Value.ToString();
    }

    /// <summary>
    ///     Override ToString method
    /// </summary>
    /// <returns>String representation of the ItemProductNumber</returns>
    public override string ToString()
    {
        return Value.ToString();
    }

    /// <summary>
    ///     Override Equals method
    /// </summary>
    /// <param name="obj">Object to compare</param>
    /// <returns>True if equal, false otherwise</returns>
    public override bool Equals(object? obj)
    {
        if (obj is ItemProductNumber other)
            return Value.Equals(other.Value);
        return false;
    }

    /// <summary>
    ///     Override GetHashCode method
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}