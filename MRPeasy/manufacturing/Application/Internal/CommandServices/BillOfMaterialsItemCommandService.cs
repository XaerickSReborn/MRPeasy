using MRPeasy.Manufacturing.Domain.Model.Aggregates;
using MRPeasy.Manufacturing.Domain.Model.Commands;
using MRPeasy.Manufacturing.Domain.Model.ValueObjects;
using MRPeasy.Manufacturing.Domain.Repositories;
using MRPeasy.Manufacturing.Domain.Services;
using MRPeasy.Shared.Domain.Repositories;

namespace MRPeasy.Manufacturing.Application.Internal.CommandServices;

/// <summary>
///     Command service for Bill of Materials Item operations
/// </summary>
public class BillOfMaterialsItemCommandService
{
    private readonly IBillOfMaterialsItemRepository _billOfMaterialsItemRepository;
    private readonly BillOfMaterialsItemDomainService _domainService;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="billOfMaterialsItemRepository">Bill of Materials Item repository</param>
    /// <param name="domainService">Domain service</param>
    /// <param name="unitOfWork">Unit of work</param>
    public BillOfMaterialsItemCommandService(
        IBillOfMaterialsItemRepository billOfMaterialsItemRepository,
        BillOfMaterialsItemDomainService domainService,
        IUnitOfWork unitOfWork)
    {
        _billOfMaterialsItemRepository = billOfMaterialsItemRepository;
        _domainService = domainService;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    ///     Handles the creation of a new Bill of Materials Item
    /// </summary>
    /// <param name="command">Create command</param>
    /// <returns>Created Bill of Materials Item or null if creation failed</returns>
    /// <exception cref="ArgumentException">Thrown when business rules are violated</exception>
    public async Task<BillOfMaterialsItem?> Handle(CreateBillOfMaterialsItemCommand command)
    {
        // Convert string to ItemProductNumber
        var itemProductNumber = ItemProductNumber.FromString(command.ItemProductNumber);

        // Validate business rules through domain service
        var (isValid, errorMessage) = await _domainService.ValidateForCreationAsync(
            command.BillOfMaterialsId, itemProductNumber, command.BatchId, command.RequiredQuantity);

        if (!isValid)
        {
            throw new ArgumentException(errorMessage);
        }

        // Create the Bill of Materials Item
        var billOfMaterialsItem = new BillOfMaterialsItem(
            command.BillOfMaterialsId,
            itemProductNumber,
            command.BatchId,
            command.RequiredQuantity,
            command.ScheduledStartAt,
            command.RequiredAt);

        try
        {
            // Add to repository
            await _billOfMaterialsItemRepository.AddAsync(billOfMaterialsItem);
            
            // Update product's current production quantity through ACL
            var updateSuccess = await _domainService.UpdateProductionQuantityAsync(
                itemProductNumber, command.RequiredQuantity);
            
            if (!updateSuccess)
            {
                throw new InvalidOperationException("Failed to update product production quantity");
            }

            // Save changes
            await _unitOfWork.CompleteAsync();
            
            return billOfMaterialsItem;
        }
        catch
        {
            // If anything fails, rollback by not completing the unit of work
            throw;
        }
    }
}