using MRPeasy.Inventories.Domain.Model.Commands;
using MRPeasy.Inventories.Interfaces.REST.Resources;

namespace MRPeasy.Inventories.Interfaces.REST.Transform;

/// <summary>
/// Ensamblador para convertir CreateProductResource a CreateProductCommand
/// </summary>
public static class CreateProductResourceToCommandAssembler
{
    /// <summary>
    /// Convierte un CreateProductResource a CreateProductCommand
    /// </summary>
    /// <param name="resource">Recurso de creación de producto</param>
    /// <returns>Comando de creación de producto</returns>
    public static CreateProductCommand ToCommandFromResource(CreateProductResource resource)
    {
        return new CreateProductCommand(
            resource.Name,
            ProductTypeConverter.FromAbbreviation(resource.ProductType),
            resource.MaxProductionCapacity
        );
    }
}