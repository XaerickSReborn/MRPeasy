using MRPeasy.Inventories.Domain.Model.ValueObjects;

namespace MRPeasy.Inventories.Interfaces.REST.Resources;

/// <summary>
/// Recurso que representa la información de un producto
/// </summary>
public class ProductResource
{
    /// <summary>
    /// Identificador único del producto
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Número de producto único generado por MRPeasy
    /// </summary>
    public string ProductNumber { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del producto
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de producto (nombre completo)
    /// </summary>
    public string ProductType { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del tipo de producto
    /// </summary>
    public string ProductTypeDescription { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad actual en producción
    /// </summary>
    public int CurrentProductionQuantity { get; set; }

    /// <summary>
    /// Capacidad máxima de producción
    /// </summary>
    public int MaxProductionCapacity { get; set; }

    /// <summary>
    /// Modo de operación del producto
    /// </summary>
    public string OperationMode { get; set; } = string.Empty;
}