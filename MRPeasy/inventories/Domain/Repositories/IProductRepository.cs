using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Domain.Model.ValueObjects;
using MRPeasy.Shared.Domain.Repositories;

namespace MRPeasy.Inventories.Domain.Repositories;

/// <summary>
/// Repositorio específico para la entidad Product
/// </summary>
public interface IProductRepository : IBaseRepository<Product>
{
    /// <summary>
    /// Busca un producto por su número de producto
    /// </summary>
    /// <param name="productNumber">Número de producto a buscar</param>
    /// <returns>El producto si existe, null en caso contrario</returns>
    Task<Product?> FindByProductNumberAsync(ProductNumber productNumber);

    /// <summary>
    /// Busca un producto por su nombre
    /// </summary>
    /// <param name="name">Nombre del producto a buscar</param>
    /// <returns>El producto si existe, null en caso contrario</returns>
    Task<Product?> FindByNameAsync(string name);

    /// <summary>
    /// Verifica si existe un producto con el número de producto especificado
    /// </summary>
    /// <param name="productNumber">Número de producto a verificar</param>
    /// <returns>True si existe, false en caso contrario</returns>
    Task<bool> ExistsByProductNumberAsync(ProductNumber productNumber);

    /// <summary>
    /// Verifica si existe un producto con el nombre especificado
    /// </summary>
    /// <param name="name">Nombre del producto a verificar</param>
    /// <returns>True si existe, false en caso contrario</returns>
    Task<bool> ExistsByNameAsync(string name);


}