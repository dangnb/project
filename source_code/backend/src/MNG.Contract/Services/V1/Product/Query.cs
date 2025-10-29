using MNG.Contract.Abstractions.Message;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Enumerations;
using static MNG.Contract.Services.V1.Product.Response;

namespace MNG.Contract.Services.V1.Product;
public static class Query
{
    public record GetProductsQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<ProductResponse>>;
    public record GetPrductByIdQuery(Guid Id) : IQuery<ProductResponse>;
}
