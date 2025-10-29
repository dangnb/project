using MNG.Contract.Abstractions.Message;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Enumerations;
using static MNG.Contract.Services.V1.Role.Response;

namespace MNG.Contract.Services.V1.Role;
public static class Query
{
    public record GetRolesQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder, int PageIndex, int PageSize) : IQuery<PagedResult<RoleResponse>>;
    public record GetRoleByIdQuery(Guid Id) : IQuery<RoleResponse>;
}
