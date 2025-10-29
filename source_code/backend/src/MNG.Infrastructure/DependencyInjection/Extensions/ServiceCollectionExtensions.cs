using Microsoft.Extensions.DependencyInjection;
using MNG.Application.Abstractions;
using MNG.Infrastructure.Authentication;

namespace MNG.Infrastructure.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureService(this IServiceCollection services)
        => services.AddTransient<IJwtTokenService, JwtTokenService>();
}
