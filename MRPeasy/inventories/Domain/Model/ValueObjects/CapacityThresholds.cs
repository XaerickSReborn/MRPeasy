namespace MRPeasy.Inventories.Domain.Model.ValueObjects;

/// <summary>
/// Configuración de umbrales de capacidad para productos
/// </summary>
public class CapacityThresholds
{
    /// <summary>
    /// Umbral mínimo de capacidad de producción
    /// </summary>
    public int MinCapacityThreshold { get; set; }

    /// <summary>
    /// Umbral máximo de capacidad de producción
    /// </summary>
    public int MaxCapacityThreshold { get; set; }

    /// <summary>
    /// Valida si un valor de capacidad está dentro de los umbrales permitidos
    /// </summary>
    /// <param name="capacity">Valor de capacidad a validar</param>
    /// <returns>True si está dentro del rango, false en caso contrario</returns>
    public bool IsValidCapacity(int capacity)
    {
        return capacity >= MinCapacityThreshold && capacity <= MaxCapacityThreshold;
    }

    /// <summary>
    /// Obtiene el mensaje de error para capacidad inválida
    /// </summary>
    /// <returns>Mensaje descriptivo del error</returns>
    public string GetCapacityErrorMessage()
    {
        return $"La capacidad máxima de producción debe estar entre {MinCapacityThreshold} y {MaxCapacityThreshold}";
    }
}