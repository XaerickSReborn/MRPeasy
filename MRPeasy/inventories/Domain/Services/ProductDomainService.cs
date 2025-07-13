using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Domain.Model.ValueObjects;
using MRPeasy.Inventories.Domain.Repositories;

namespace MRPeasy.Inventories.Domain.Services;

/// <summary>
/// Servicio de dominio para manejar las reglas de negocio complejas de Product
/// </summary>
public class ProductDomainService
{
    private readonly IProductRepository _productRepository;

    /// <summary>
    /// Constructor del servicio de dominio de productos
    /// </summary>
    /// <param name="productRepository">Repositorio de productos</param>
    public ProductDomainService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Valida que un producto pueda ser creado según las reglas de negocio
    /// </summary>
    /// <param name="name">Nombre del producto</param>
    /// <param name="productType">Tipo de producto</param>
    /// <param name="maxProductionCapacity">Capacidad máxima de producción</param>
    /// <param name="capacityThresholds">Configuración de umbrales</param>
    /// <returns>El producto creado si es válido</returns>
    /// <exception cref="InvalidOperationException">Si viola alguna regla de negocio</exception>
    public async Task<Product> CreateProductAsync(string name, EProductType productType, int maxProductionCapacity, CapacityThresholds capacityThresholds)
    {
        // Validar unicidad del nombre
        await ValidateUniqueNameAsync(name);

        // Crear el producto (las validaciones internas se ejecutan en el constructor)
        var product = new Product(name, productType, maxProductionCapacity, capacityThresholds);

        return product;
    }



    /// <summary>
    /// Valida que el nombre del producto sea único en el sistema
    /// </summary>
    /// <param name="name">Nombre a validar</param>
    /// <exception cref="InvalidOperationException">Si el nombre ya existe</exception>
    private async Task ValidateUniqueNameAsync(string name)
    {
        var nameExists = await _productRepository.ExistsByNameAsync(name);

        if (nameExists)
        {
            throw new InvalidOperationException($"Ya existe un producto con el nombre '{name.Trim()}'. El nombre debe ser único.");
        }
    }

    /// <summary>
    /// Valida que un ProductNumber sea único en el sistema
    /// </summary>
    /// <param name="productNumber">Número de producto a validar</param>
    /// <exception cref="InvalidOperationException">Si el número de producto ya existe</exception>
    public async Task ValidateUniqueProductNumberAsync(ProductNumber productNumber)
    {
        var exists = await _productRepository.ExistsByProductNumberAsync(productNumber);
        if (exists)
        {
            throw new InvalidOperationException($"Ya existe un producto con el número '{productNumber}'. El número de producto debe ser único.");
        }
    }
}