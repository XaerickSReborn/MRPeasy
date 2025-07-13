using MRPeasy.Inventories.Application.Internal.CommandServices;
using MRPeasy.Inventories.Application.Internal.QueryServices;
using MRPeasy.Inventories.Domain.Model.Queries;
using MRPeasy.Inventories.Interfaces.REST.Resources;
using MRPeasy.Inventories.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace MRPeasy.Inventories.Interfaces.REST;

/// <summary>
/// Controlador REST para la gestión de productos
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints para la gestión de productos en el sistema MRPeasy")]
public class ProductsController : ControllerBase
{
    private readonly ProductCommandService _productCommandService;
    private readonly ProductQueryService _productQueryService;

    /// <summary>
    /// Constructor del controlador de productos
    /// </summary>
    /// <param name="productCommandService">Servicio de comandos de productos</param>
    /// <param name="productQueryService">Servicio de consultas de productos</param>
    public ProductsController(
        ProductCommandService productCommandService,
        ProductQueryService productQueryService)
    {
        _productCommandService = productCommandService;
        _productQueryService = productQueryService;
    }

    /// <summary>
    /// Obtiene un producto por su identificador
    /// </summary>
    /// <param name="id">Identificador del producto</param>
    /// <returns>Información del producto</returns>
    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Obtiene un producto por ID",
        Description = "Retorna la información detallada de un producto específico",
        OperationId = "GetProductById")]
    [SwaggerResponse(200, "Producto encontrado", typeof(ProductResource))]
    [SwaggerResponse(404, "Producto no encontrado")]
    public async Task<ActionResult<ProductResource>> GetProductById(int id)
    {
        var getProductByIdQuery = new GetProductByIdQuery(id);
        var product = await _productQueryService.Handle(getProductByIdQuery);
        
        if (product == null)
            return NotFound($"Producto con ID {id} no encontrado");

        var resource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
        return Ok(resource);
    }



    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    /// <param name="resource">Información del producto a crear</param>
    /// <returns>Producto creado</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Crea un nuevo producto",
        Description = "Registra un nuevo producto en el sistema con la información proporcionada",
        OperationId = "CreateProduct")]
    [SwaggerResponse(201, "Producto creado exitosamente", typeof(ProductResource))]
    [SwaggerResponse(400, "Datos de entrada inválidos")]
    [SwaggerResponse(409, "Conflicto - El nombre del producto ya existe")]
    public async Task<ActionResult<ProductResource>> CreateProduct([FromBody] CreateProductResource resource)
    {
        try
        {
            var createProductCommand = CreateProductResourceToCommandAssembler.ToCommandFromResource(resource);
            var product = await _productCommandService.Handle(createProductCommand);
            var productResource = ProductResourceFromEntityAssembler.ToResourceFromEntity(product);
            
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productResource);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}