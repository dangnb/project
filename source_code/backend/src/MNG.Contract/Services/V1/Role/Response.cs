using System.Collections.Generic;

namespace MNG.Contract.Services.V1.Role;
public static class Response
{
    public record RoleResponse(Guid Id, string RoleCode, decimal Description, Dictionary<string, int> permissions);
}
