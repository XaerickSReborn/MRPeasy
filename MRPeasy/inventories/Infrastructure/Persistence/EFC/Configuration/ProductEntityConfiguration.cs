using MRPeasy.Inventories.Domain.Model.Aggregates;
using MRPeasy.Inventories.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRPeasy.Inventories.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
/// Configuración de Entity Framework para la entidad Product
/// </summary>
public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
{
    /// <summary>
    /// Configura la entidad Product para Entity Framework
    /// </summary>
    /// <param name="builder">Constructor de configuración de entidad</param>
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Configuración de la tabla
        builder.ToTable("products");

        // Configuración de la clave primaria
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // Configuración del ProductNumber como Value Object
        builder.Property(p => p.ProductNumber)
            .HasColumnName("product_number")
            .HasColumnType("char(36)")
            .IsRequired()
            .HasConversion(
                productNumber => productNumber.Value,
                value => ProductNumber.FromValue(value)
            );

        // Índice único para ProductNumber
        builder.HasIndex(p => p.ProductNumber)
            .IsUnique()
            .HasDatabaseName("ix_products_product_number_unique");

        // Configuración del nombre
        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(60)
            .IsRequired();

        // Índice único para Name
        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName("ix_products_name_unique");

        // Configuración del tipo de producto
        builder.Property(p => p.ProductType)
            .HasColumnName("product_type")
            .HasConversion<int>()
            .IsRequired();

        // Configuración de la cantidad actual en producción
        builder.Property(p => p.CurrentProductionQuantity)
            .HasColumnName("current_production_quantity")
            .IsRequired()
            .HasDefaultValue(0);

        // Configuración de la capacidad máxima de producción
        builder.Property(p => p.MaxProductionCapacity)
            .HasColumnName("max_production_capacity")
            .IsRequired();

        // Índice para consultas por tipo de producto
        builder.HasIndex(p => p.ProductType)
            .HasDatabaseName("ix_products_product_type");

        // Índice para consultas por cantidad en producción
        builder.HasIndex(p => p.CurrentProductionQuantity)
            .HasDatabaseName("ix_products_current_production_quantity");

        // Configuración de campos de auditoría
        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Índices para campos de auditoría
        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName("ix_products_created_at");

        builder.HasIndex(p => p.UpdatedAt)
            .HasDatabaseName("ix_products_updated_at");
    }
}