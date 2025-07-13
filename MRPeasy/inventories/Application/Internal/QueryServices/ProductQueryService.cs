using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Domain.Model.Queries;
using MRPeasy.Inventories.Domain.Repositories;

namespace MRPeasy.Inventories.Application.Internal.QueryServices;

/// <summary>
/// Servicio de aplicación para manejar consultas relacionadas con productos
/// </summary>
public class ProductQueryService
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Constructor del servicio de consultas de productos
    /// </summary>
    /// <param name="productRepository">Repositorio de productos</param>
    public ProductQueryService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Maneja la consulta para obtener un producto por su identificador
    /// </summary>
    /// <param name="query">Query con el identificador del producto</param>
    /// <returns>El producto si existe, null en caso contrario</returns>
    public async Task<Product?> Handle(GetProductByIdQuery query)
    {
        return await _productRepository.FindByIdAsync(query.Id);
    }


}