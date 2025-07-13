using MRPeasy.Inventories.Domain.Model.ValueObjects;

namespace MRPeasy.Inventories.Domain.Model.Commands;

/// <summary>
/// Comando para crear un nuevo producto
/// </summary>
/// <param name="Name">Nombre del producto</param>
/// <param name="ProductType">Tipo de producto</param>
/// <param name="MaxProductionCapacity">Capacidad máxima de producción</param>
public record CreateProductCommand(
    string Name,
    EProductType ProductType,
    int MaxProductionCapacity
);