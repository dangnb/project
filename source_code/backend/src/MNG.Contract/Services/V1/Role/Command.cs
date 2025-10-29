using MNG.Contract.Abstractions.Message;

namespace MNG.Contract.Services.V1.Role;
public static class Command
{
    public record CreateRoleCommand(string RoleCode, string RoleName, string Description, Dictionary<string, int> Permissions) : ICommand;
    public record UpdateRoleCommand(Guid Id, string RoleCode, string RoleName, string Description, Dictionary<string, int> Permissions) : ICommand;
    public record DeleteRoleCommand(Guid Id) : ICommand;
}

