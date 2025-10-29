using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MNG.Application.Abstractions;
using MNG.Contract.Abstractions.Message;
using MNG.Contract.Abstractions.Shared;
using MNG.Contract.Services.V1.Identity;
using MNG.Domain.Entities.Identity;
using MNG.Domain.Utilities.Constants;
using Newtonsoft.Json;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;

namespace MNG.Application.UserCases.V1.Queries.Identity;
public class GetLoginQueryHandler : IQueryHandler<Query.Login, Response.Authenticated>
{
    private readonly UserManager<AppUser> _userManeger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly RoleManager<AppRole> _roleManager;

    public GetLoginQueryHandler(IJwtTokenService jwtTokenService, UserManager<AppUser> userManeger, RoleManager<AppRole> roleManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManeger = userManeger;
        _roleManager = roleManager;
    }

    public async Task<Result<Response.Authenticated>> Handle(Query.Login request, CancellationToken cancellationToken)
    {
        //Check user token xem co ton tai hay khong
        var user = await _userManeger.FindByNameAsync(request.Email) ?? throw new Exception("Không tìm thấy user");

        var isLockedOut = await _userManeger.IsLockedOutAsync(user);
        if (isLockedOut)
        {
            throw new Exception("Tài khoản đã bị khóa");
        }

        var checkPassword = await _userManeger.CheckPasswordAsync(user, request.Password);
        if (!checkPassword)
        {
            await _userManeger.AccessFailedAsync(user);
            throw new Exception("Mật khẩu không chính xác");
        }



        //Generate jwt token
        var claims = new List<Claim>()
        {
            new (ClaimTypes.Email,user.Email ),
            new (ClaimTypes.Role,JsonConvert.SerializeObject(GetRolesByUser(user)) ),
            new (ClaimTypes.GivenName,user.FullName ),
            new (ClaimTypes.NameIdentifier,user.Id.ToString() ),
            new (ClaimTypes.Name,user.UserName ),
        };

        var token = _jwtTokenService.GenerateAccessToken(claims);

        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var response = new Response.Authenticated() { AccessToken = token, RefreshToken = refreshToken, RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5) };
        return Result.Success(response);
    }

    private async Task<Dictionary<string, int>> GetRolesByUser(AppUser user)
    {
        var results = new Dictionary<string, int>();
        var userClaims = await _userManeger.GetClaimsAsync(user);
        var roleClaims = await GetClaimByUserAsync(user);
        if (userClaims.Any())
        {
            roleClaims.AddRange(userClaims);
        }
        var roleClaimNames = roleClaims.Select(x => x.Type).Distinct();
        foreach (var name in roleClaimNames)
        {
            if (results.ContainsKey(name))
                continue;

            var claims = roleClaims.Where(x => x.Type == name);
            if (claims.Any(p => p.Value == PermissionConstants.Deny.ToString()))
                continue;

            var value = 0;
            foreach (var claim in claims)
            {
                if (int.TryParse(claim.Value, out int claimValue))
                {
                    value |= claimValue;
                }
                results.Add(name, value);
            }
        }

        return results;
    }

    private async Task<List<Claim>> GetClaimByUserAsync(AppUser user)
    {
        var roleNames = await _userManeger.GetRolesAsync(user);
        var roles = await _roleManager.Roles.Where(x => roleNames.Contains(x.Name)).ToListAsync();
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
        {
            var resultClaims = await _roleManager.GetClaimsAsync(role);
            if (resultClaims.Any())
            {
                roleClaims.AddRange(resultClaims);
            }

        }
        return roleClaims;

    }
}
