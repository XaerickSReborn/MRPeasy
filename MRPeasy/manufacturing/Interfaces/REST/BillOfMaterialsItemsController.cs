using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MRPeasy.Manufacturing.Application.Internal.CommandServices;
using MRPeasy.Manufacturing.Interfaces.REST.Resources;
using MRPeasy.Manufacturing.Interfaces.REST.Transform;
using Swashbuckle.AspNetCore.Annotations;

namespace MRPeasy.Manufacturing.Interfaces.REST;

/// <summary>
///     Controller for Bill of Materials Items operations
/// </summary>
[ApiController]
[Route("api/v1/bill-of-materials/{billOfMaterialsId}/items")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Bill of Materials Items")]
public class BillOfMaterialsItemsController : ControllerBase
{
    private readonly BillOfMaterialsItemCommandService _billOfMaterialsItemCommandService;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="billOfMaterialsItemCommandService">Command service for Bill of Materials Items</param>
    public BillOfMaterialsItemsController(BillOfMaterialsItemCommandService billOfMaterialsItemCommandService)
    {
        _billOfMaterialsItemCommandService = billOfMaterialsItemCommandService;
    }

    /// <summary>
    ///     Creates a new Bill of Materials Item
    /// </summary>
    /// <param name="billOfMaterialsId">Bill of Materials ID from route</param>
    /// <param name="resource">Bill of Materials Item creation data</param>
    /// <returns>Created Bill of Materials Item</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a Bill of Materials Item",
        Description = "Creates a new Bill of Materials Item with the provided information",
        OperationId = "CreateBillOfMaterialsItem")]
    [SwaggerResponse(201, "Bill of Materials Item was created", typeof(BillOfMaterialsItemResource))]
    [SwaggerResponse(400, "Bad request")]
    [SwaggerResponse(409, "Conflict - Bill of Materials Item with same combination already exists")]
    public async Task<IActionResult> CreateBillOfMaterialsItem(
        [FromRoute] int billOfMaterialsId,
        [FromBody] CreateBillOfMaterialsItemResource resource)
    {
        try
        {
            var createCommand = CreateBillOfMaterialsItemCommandFromResourceAssembler
                .ToCommandFromResource(billOfMaterialsId, resource);
            
            var billOfMaterialsItem = await _billOfMaterialsItemCommandService.Handle(createCommand);
            
            if (billOfMaterialsItem == null)
            {
                return BadRequest("Failed to create Bill of Materials Item");
            }

            var billOfMaterialsItemResource = BillOfMaterialsItemResourceFromEntityAssembler
                .ToResourceFromEntity(billOfMaterialsItem);
            
            return CreatedAtAction(nameof(CreateBillOfMaterialsItem), new { billOfMaterialsId }, billOfMaterialsItemResource);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
}