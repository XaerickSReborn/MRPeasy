using System.ComponentModel.DataAnnotations;

namespace MRPeasy.Shared.Domain.Model;

/// <summary>
/// Clase base para agregados con atributos de auditoría
/// </summary>
public abstract class AuditableAggregateRoot
{
    /// <summary>
    /// Fecha y hora de creación del registro
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Fecha y hora de última actualización del registro
    /// </summary>
    [Required]
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Constructor protegido para inicializar las fechas de auditoría
    /// </summary>
    protected AuditableAggregateRoot()
    {
        var now = DateTime.UtcNow;
        CreatedAt = now;
        UpdatedAt = now;
    }

    /// <summary>
    /// Actualiza la fecha de última modificación
    /// </summary>
    protected void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Método para ser llamado antes de guardar cambios
    /// </summary>
    public virtual void OnBeforeSave()
    {
        UpdateTimestamp();
    }
}