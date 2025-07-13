using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Domain.Model.ValueObjects;
using MRPeasy.Inventories.Interfaces.REST.Resources;

namespace MRPeasy.Inventories.Interfaces.REST.Transform;

/// <summary>
/// Ensamblador para convertir Product entity a ProductResource
/// </summary>
public static class ProductResourceFromEntityAssembler
{
    /// <summary>
    /// Convierte una entidad Product a ProductResource
    /// </summary>
    /// <param name="entity">Entidad Product</param>
    /// <returns>Recurso ProductResource</returns>
    public static ProductResource ToResourceFromEntity(Product entity)
    {
        return new ProductResource
        {
            Id = entity.Id,
            ProductNumber = entity.ProductNumber.ToString(),
            Name = entity.Name,
            ProductType = ProductTypeConverter.ToFullName(entity.ProductType),
            ProductTypeDescription = GetProductTypeDescription(entity.ProductType),
            CurrentProductionQuantity = entity.CurrentProductionQuantity,
            MaxProductionCapacity = entity.MaxProductionCapacity,
            OperationMode = GetProductTypeDescription(entity.ProductType)
        };
    }

    /// <summary>
    /// Convierte una lista de entidades Product a una lista de ProductResource
    /// </summary>
    /// <param name="entities">Lista de entidades Product</param>
    /// <returns>Lista de recursos ProductResource</returns>
    public static IEnumerable<ProductResource> ToResourceFromEntity(IEnumerable<Product> entities)
    {
        return entities.Select(ToResourceFromEntity);
    }

    /// <summary>
    /// Obtiene la descripción textual del tipo de producto
    /// </summary>
    /// <param name="productType">Tipo de producto</param>
    /// <returns>Descripción del tipo de producto</returns>
    private static string GetProductTypeDescription(EProductType productType)
    {
        return productType switch
        {
            EProductType.BuildToPrint => "Construir según planos",
            EProductType.BuildToSpecification => "Construir según especificaciones",
            EProductType.MadeToStock => "Fabricado para stock",
            EProductType.MadeToOrder => "Fabricado bajo pedido",
            EProductType.MadeToAssemble => "Fabricado para ensamblar",
            _ => "Tipo desconocido"
        };
    }
}