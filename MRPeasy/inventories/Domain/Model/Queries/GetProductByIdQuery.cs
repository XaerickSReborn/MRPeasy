namespace MRPeasy.Inventories.Domain.Model.Queries;

/// <summary>
/// Query para obtener un producto por su identificador
/// </summary>
/// <param name="Id">Identificador del producto</param>
public record GetProductByIdQuery(int Id);