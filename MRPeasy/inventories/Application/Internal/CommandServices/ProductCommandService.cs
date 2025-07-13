using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Domain.Model.Commands;
using MRPeasy.Inventories.Domain.Model.ValueObjects;
using MRPeasy.Inventories.Domain.Repositories;
using MRPeasy.Inventories.Domain.Services;
using MRPeasy.Shared.Domain.Repositories;
using Microsoft.Extensions.Options;

namespace MRPeasy.Inventories.Application.Internal.CommandServices;

/// <summary>
/// Servicio de aplicación para manejar comandos relacionados con productos
/// </summary>
public class ProductCommandService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductDomainService _productDomainService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CapacityThresholds _capacityThresholds;

    /// <summary>
    /// Constructor del servicio de comandos de productos
    /// </summary>
    /// <param name="productRepository">Repositorio de productos</param>
    /// <param name="productDomainService">Servicio de dominio de productos</param>
    /// <param name="unitOfWork">Unidad de trabajo</param>
    /// <param name="capacityThresholds">Configuración de umbrales de capacidad</param>
    public ProductCommandService(
        IProductRepository productRepository,
        ProductDomainService productDomainService,
        IUnitOfWork unitOfWork,
        IOptions<CapacityThresholds> capacityThresholds)
    {
        _productRepository = productRepository;
        _productDomainService = productDomainService;
        _unitOfWork = unitOfWork;
        _capacityThresholds = capacityThresholds.Value;
    }

    /// <summary>
    /// Maneja el comando de creación de un nuevo producto
    /// </summary>
    /// <param name="command">Comando de creación de producto</param>
    /// <returns>El producto creado</returns>
    /// <exception cref="InvalidOperationException">Si viola alguna regla de negocio</exception>
    public async Task<Product> Handle(CreateProductCommand command)
    {
        // Crear el producto usando el servicio de dominio (incluye validaciones)
        var product = await _productDomainService.CreateProductAsync(
            command.Name,
            command.ProductType,
            command.MaxProductionCapacity,
            _capacityThresholds
        );

        // Agregar al repositorio
        await _productRepository.AddAsync(product);

        // Guardar cambios
        await _unitOfWork.CompleteAsync();

        return product;
    }


}