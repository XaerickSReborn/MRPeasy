using System.ComponentModel.DataAnnotations;

namespace MRPeasy.Inventories.Interfaces.REST.Resources;

/// <summary>
/// Recurso para crear un nuevo producto
/// </summary>
public class CreateProductResource
{
    /// <summary>
    /// Nombre del producto
    /// </summary>
    [Required(ErrorMessage = "El nombre del producto es requerido")]
    [StringLength(60, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 60 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Tipo de producto (siglas: BTP, BTS, MTS, MTO, MTA)
    /// </summary>
    [Required(ErrorMessage = "El tipo de producto es requerido")]
    [RegularExpression("^(BTP|BTS|MTS|MTO|MTA)$", ErrorMessage = "Tipo de producto inválido. Valores válidos: BTP, BTS, MTS, MTO, MTA")]
    public string ProductType { get; set; } = string.Empty;

    /// <summary>
    /// Capacidad máxima de producción
    /// </summary>
    [Required(ErrorMessage = "La capacidad máxima de producción es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La capacidad máxima debe ser mayor a 0")]
    public int MaxProductionCapacity { get; set; }
}