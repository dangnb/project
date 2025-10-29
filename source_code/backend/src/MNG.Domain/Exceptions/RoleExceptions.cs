namespace MNG.Domain.Exceptions;
public class RoleExceptions
{
    public class RoleValidNameException : BadRequestException
    {
        public RoleValidNameException(string roleName)
            : base($"Quyền {roleName} đã tồn tại trên hệ thống.") { }
    }
}
