using MRPeasy.Inventories.Domain.Model.ValueObjects;
using MRPeasy.Shared.Domain.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRPeasy.Inventories.Domain.Model.Aggregates;

/// <summary>
/// Entidad que representa un producto en el sistema MRPeasy
/// </summary>
public partial class Product : AuditableAggregateRoot
{
    /// <summary>
    /// Identificador único del producto (Primary Key)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private set; }

    /// <summary>
    /// Número de producto único generado por MRPeasy
    /// </summary>
    [Required]
    [Column(TypeName = "char(36)")]
    public ProductNumber ProductNumber { get; private set; } = null!;

    /// <summary>
    /// Nombre del producto
    /// </summary>
    [Required]
    [MaxLength(60)]
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Tipo de producto que determina el modo de operación
    /// </summary>
    [Required]
    public EProductType ProductType { get; private set; }

    /// <summary>
    /// Cantidad actual en producción
    /// </summary>
    [Required]
    public int CurrentProductionQuantity { get; protected set; }

    /// <summary>
    /// Capacidad máxima de producción
    /// </summary>
    [Required]
    public int MaxProductionCapacity { get; private set; }

    /// <summary>
    /// Constructor privado para Entity Framework
    /// </summary>
    private Product() : base() { }

    /// <summary>
    /// Constructor para crear un nuevo producto
    /// </summary>
    /// <param name="name">Nombre del producto</param>
    /// <param name="productType">Tipo de producto</param>
    /// <param name="maxProductionCapacity">Capacidad máxima de producción</param>
    /// <param name="capacityThresholds">Configuración de umbrales de capacidad</param>
    public Product(string name, EProductType productType, int maxProductionCapacity, CapacityThresholds capacityThresholds) : base()
    {
        ValidateName(name);
        ValidateMaxProductionCapacity(maxProductionCapacity, capacityThresholds);

        ProductNumber = ProductNumber.Generate();
        Name = name.Trim();
        ProductType = productType;
        CurrentProductionQuantity = 0; // Valor por defecto según especificaciones
        MaxProductionCapacity = maxProductionCapacity;
    }



    /// <summary>
    /// Valida el nombre del producto según las reglas de negocio
    /// </summary>
    /// <param name="name">Nombre a validar</param>
    /// <exception cref="ArgumentException">Si el nombre no cumple las reglas</exception>
    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del producto es requerido y no puede estar vacío", nameof(name));

        if (name.Trim().Length > 60)
            throw new ArgumentException("El nombre del producto no puede exceder 60 caracteres", nameof(name));
    }

    /// <summary>
    /// Valida la capacidad máxima de producción según los umbrales configurados
    /// </summary>
    /// <param name="maxProductionCapacity">Capacidad a validar</param>
    /// <param name="capacityThresholds">Configuración de umbrales</param>
    /// <exception cref="ArgumentException">Si la capacidad no está dentro de los umbrales</exception>
    private static void ValidateMaxProductionCapacity(int maxProductionCapacity, CapacityThresholds capacityThresholds)
    {
        if (!capacityThresholds.IsValidCapacity(maxProductionCapacity))
            throw new ArgumentException(capacityThresholds.GetCapacityErrorMessage(), nameof(maxProductionCapacity));
    }

    /// <summary>
    /// Actualiza la cantidad actual de producción
    /// </summary>
    /// <param name="quantityToAdd">Cantidad a agregar (puede ser negativa para restar)</param>
    /// <returns>True si la actualización fue exitosa, false si excedería la capacidad</returns>
    public bool UpdateCurrentProductionQuantity(int quantityToAdd)
    {
        var newQuantity = CurrentProductionQuantity + quantityToAdd;
        
        if (newQuantity < 0 || newQuantity > MaxProductionCapacity)
            return false;
            
        CurrentProductionQuantity = newQuantity;
        OnBeforeSave(); // Actualizar timestamp de auditoría
        return true;
    }
}