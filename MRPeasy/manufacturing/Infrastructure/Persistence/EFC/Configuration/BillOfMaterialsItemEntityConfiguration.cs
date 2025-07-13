using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRPeasy.Manufacturing.Domain.Model.Aggregates;
using MRPeasy.Manufacturing.Domain.Model.ValueObjects;
using MRPeasy.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace MRPeasy.Manufacturing.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Entity Framework configuration for Bill of Materials Item
/// </summary>
public class BillOfMaterialsItemEntityConfiguration : IEntityTypeConfiguration<BillOfMaterialsItem>
{
    /// <summary>
    ///     Configures the Bill of Materials Item entity
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<BillOfMaterialsItem> builder)
    {
        // Configure table name
        builder.ToTable("bill_of_materials_items");

        // Configure primary key
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Configure BillOfMaterialsId
        builder.Property(item => item.BillOfMaterialsId)
            .HasColumnName("bill_of_materials_id")
            .IsRequired();

        // Configure ItemProductNumber as Value Object with conversion
        builder.Property(e => e.ItemProductNumber)
            .HasColumnName("item_product_number")
            .HasColumnType("char(36)")
            .IsRequired()
            .HasConversion(
                itemProductNumber => itemProductNumber.Value,
                value => ItemProductNumber.FromValue(value)
            );

        // Configure BatchId
        builder.Property(item => item.BatchId)
            .HasColumnName("batch_id")
            .IsRequired();

        // Configure RequiredQuantity
        builder.Property(item => item.RequiredQuantity)
            .HasColumnName("required_quantity")
            .IsRequired();

        // Configure ScheduledStartAt
        builder.Property(item => item.ScheduledStartAt)
            .HasColumnName("scheduled_start_at")
            .IsRequired();

        // Configure RequiredAt
        builder.Property(item => item.RequiredAt)
            .HasColumnName("required_at")
            .IsRequired();



        // Configure indexes
        builder.HasIndex(e => e.BillOfMaterialsId)
            .HasDatabaseName("ix_bill_of_materials_items_bill_of_materials_id");
        
        // Create unique index on combination
        builder.HasIndex(e => new { e.ItemProductNumber, e.BatchId, e.BillOfMaterialsId })
            .IsUnique()
            .HasDatabaseName("uq_bill_of_materials_items_combination");
            
        builder.HasIndex(e => e.ItemProductNumber)
            .HasDatabaseName("ix_bill_of_materials_items_item_product_number");

        // Configure audit fields
        builder.Property(item => item.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(item => item.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Indexes for audit fields
        builder.HasIndex(item => item.CreatedAt)
            .HasDatabaseName("ix_bill_of_materials_items_created_at");

        builder.HasIndex(item => item.UpdatedAt)
            .HasDatabaseName("ix_bill_of_materials_items_updated_at");

        // Note: Snake case naming convention is applied at the ModelBuilder level in AppDbContext
    }
}