namespace MRPeasy.Inventories.Domain.Model.ValueObjects;

/// <summary>
/// Value Object que representa el número de producto único generado por MRPeasy
/// </summary>
public record ProductNumber
{
    /// <summary>
    /// Valor UUID que identifica únicamente el producto
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// Constructor privado para crear un ProductNumber con un valor específico
    /// </summary>
    /// <param name="value">El valor UUID del número de producto</param>
    private ProductNumber(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("Product number cannot be empty", nameof(value));
        
        Value = value;
    }

    /// <summary>
    /// Genera un nuevo ProductNumber con un UUID único
    /// </summary>
    /// <returns>Nueva instancia de ProductNumber</returns>
    public static ProductNumber Generate()
    {
        return new ProductNumber(Guid.NewGuid());
    }

    /// <summary>
    /// Crea un ProductNumber a partir de un valor UUID existente
    /// </summary>
    /// <param name="value">El valor UUID</param>
    /// <returns>Instancia de ProductNumber</returns>
    public static ProductNumber FromValue(Guid value)
    {
        return new ProductNumber(value);
    }

    /// <summary>
    /// Crea un ProductNumber a partir de una cadena UUID
    /// </summary>
    /// <param name="value">La cadena UUID</param>
    /// <returns>Instancia de ProductNumber</returns>
    public static ProductNumber FromString(string value)
    {
        if (!Guid.TryParse(value, out var guid))
            throw new ArgumentException("Invalid UUID format", nameof(value));
        
        return new ProductNumber(guid);
    }

    /// <summary>
    /// Convierte el ProductNumber a string
    /// </summary>
    /// <returns>Representación en string del UUID</returns>
    public override string ToString()
    {
        return Value.ToString();
    }

    /// <summary>
    /// Conversión implícita a Guid
    /// </summary>
    /// <param name="productNumber">El ProductNumber a convertir</param>
    /// <returns>El valor Guid</returns>
    public static implicit operator Guid(ProductNumber productNumber)
    {
        return productNumber.Value;
    }

    /// <summary>
    /// Conversión implícita a string
    /// </summary>
    /// <param name="productNumber">El ProductNumber a convertir</param>
    /// <returns>El valor como string</returns>
    public static implicit operator string(ProductNumber productNumber)
    {
        return productNumber.ToString();
    }
}