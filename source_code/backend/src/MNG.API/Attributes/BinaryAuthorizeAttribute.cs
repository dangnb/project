using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MNG.Domain.Enums;
using Newtonsoft.Json;

namespace MNG.API.Attributes;

public class BinaryAuthorizeAttribute : TypeFilterAttribute
{
    public string RoleName { get; set; }
    public ActionType ActionValue { get; set; }
    public BinaryAuthorizeAttribute(string roleName, ActionType actionvalue) : base(typeof(BinaryAuthorizeFilter))
    {
        RoleName = roleName;
        ActionValue = actionvalue;
        Arguments = [RoleName, ActionValue];
    }
  
}

public class BinaryAuthorizeFilter : IAuthorizationFilter
{
    public string RoleName { get; set; }
    public ActionType ActionValue { get; set; }

    public BinaryAuthorizeFilter(string roleName, ActionType actionvalue)
    {
        RoleName = roleName;
        ActionValue = actionvalue;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasRole = CanAccessToAction(context.HttpContext);
        if (!hasRole)
            context.Result = new ForbidResult();
    }

    private bool CanAccessToAction(HttpContext httpContext)
    {
        if (!httpContext.User.Identity.IsAuthenticated)
            return false;

        if (string.IsNullOrWhiteSpace(RoleName))
            return true;

        var claimRoleValue = httpContext.User.FindFirstValue(ClaimTypes.Role);
        if (string.IsNullOrEmpty(claimRoleValue))
            return false;

        var log = httpContext.RequestServices.GetRequiredService<ILogger<BinaryAuthorizeFilter>>();
        try
        {
            var roles = JsonConvert.DeserializeObject<Dictionary<string, int>>(claimRoleValue);
            var actionValue = (int)ActionValue;
            foreach (var name in RoleName.Split(new char[] { ',', ';' }))
            {
                if (roles.TryGetValue(name, out int roleValue))
                {
                    var checkValue = (roleValue & actionValue) == actionValue;
                    if (checkValue)
                        return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            log.LogError(ex.Message);
            return false;
        }

    }
}
