namespace MRPeasy.Inventories.Domain.Model.ValueObjects;

/// <summary>
/// Enumeración que representa el tipo de producto y su modo de operación
/// </summary>
public enum EProductType
{
    /// <summary>
    /// Construir según planos/especificaciones técnicas
    /// </summary>
    BuildToPrint = 0,
    
    /// <summary>
    /// Construir según especificaciones del cliente
    /// </summary>
    BuildToSpecification = 1,
    
    /// <summary>
    /// Fabricado para mantener en stock
    /// </summary>
    MadeToStock = 2,
    
    /// <summary>
    /// Fabricado bajo pedido específico
    /// </summary>
    MadeToOrder = 3,
    
    /// <summary>
    /// Fabricado para ensamblar con otros componentes
    /// </summary>
    MadeToAssemble = 4
}