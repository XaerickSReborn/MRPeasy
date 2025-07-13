using MRPeasy.Inventories.Domain.Model.ValueObjects;
using System.ComponentModel;
using System.Globalization;

namespace MRPeasy.Inventories.Interfaces.REST.Transform;

/// <summary>
/// Convertidor para manejar ProductType como strings en requests y responses
/// </summary>
public static class ProductTypeConverter
{
    /// <summary>
    /// Convierte una sigla de ProductType a enum
    /// </summary>
    /// <param name="abbreviation">Sigla del tipo de producto (BTP, BTS, MTS, MTO, MTA)</param>
    /// <returns>Enum EProductType correspondiente</returns>
    /// <exception cref="ArgumentException">Si la sigla no es válida</exception>
    public static EProductType FromAbbreviation(string abbreviation)
    {
        return abbreviation?.ToUpperInvariant() switch
        {
            "BTP" => EProductType.BuildToPrint,
            "BTS" => EProductType.BuildToSpecification,
            "MTS" => EProductType.MadeToStock,
            "MTO" => EProductType.MadeToOrder,
            "MTA" => EProductType.MadeToAssemble,
            _ => throw new ArgumentException($"Invalid product type abbreviation: {abbreviation}. Valid values are: BTP, BTS, MTS, MTO, MTA")
        };
    }

    /// <summary>
    /// Convierte un enum EProductType a su nombre completo
    /// </summary>
    /// <param name="productType">Enum EProductType</param>
    /// <returns>Nombre completo del tipo de producto</returns>
    public static string ToFullName(EProductType productType)
    {
        return productType switch
        {
            EProductType.BuildToPrint => "BuildToPrint",
            EProductType.BuildToSpecification => "BuildToSpecification",
            EProductType.MadeToStock => "MadeToStock",
            EProductType.MadeToOrder => "MadeToOrder",
            EProductType.MadeToAssemble => "MadeToAssemble",
            _ => throw new ArgumentException($"Unknown product type: {productType}")
        };
    }

    /// <summary>
    /// Convierte un enum EProductType a su sigla
    /// </summary>
    /// <param name="productType">Enum EProductType</param>
    /// <returns>Sigla del tipo de producto</returns>
    public static string ToAbbreviation(EProductType productType)
    {
        return productType switch
        {
            EProductType.BuildToPrint => "BTP",
            EProductType.BuildToSpecification => "BTS",
            EProductType.MadeToStock => "MTS",
            EProductType.MadeToOrder => "MTO",
            EProductType.MadeToAssemble => "MTA",
            _ => throw new ArgumentException($"Unknown product type: {productType}")
        };
    }
}