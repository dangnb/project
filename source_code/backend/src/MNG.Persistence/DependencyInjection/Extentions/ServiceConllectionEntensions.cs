using MNG.Domain.Entities.Identity;
using MNG.Domain.Abstractions;
using MNG.Domain.Abstractions.Repositories;
using MNG.Persistence.DependencyInjection.Options;
using MNG.Persistence.Repositoty;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MNG.Persistence;
using Microsoft.AspNetCore.Http;


namespace MNG.Persistence.DependencyInjection.Extentions;
public static class ServiceConllectionEntensions
{
    public static void AddSqlConfiguration(this IServiceCollection services)
    {
        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var options = provider.GetRequiredService<IOptionsMonitor<MySqlRetryOptions>>();

            #region ============== SQL-SERVER-STRATEGY-1 ==============
            builder
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(true)
            .UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            .UseMySql(
                 configuration.GetConnectionString("ConnectionStrings"),
                 new MySqlServerVersion(new Version(8, 0, 21)),
                         optionsBuilder
                  => optionsBuilder.ExecutionStrategy(
                          dependencies => new MySqlRetryingExecutionStrategy(
                              dependencies: dependencies,
                              maxRetryCount: options.CurrentValue.MaxRetryCount,
                              maxRetryDelay: options.CurrentValue.MaxRetryDelay,
                              errorNumbersToAdd: options.CurrentValue.ErrorNumbersToAdd))
                      .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name)
                 );

            #endregion ============== SQL-SERVER-STRATEGY-1 ==============

            #region ============== SQL-SERVER-STRATEGY-2 ==============

            //builder
            //.EnableDetailedErrors(true)
            //.EnableSensitiveDataLogging(true)
            //.UseLazyLoadingProxies(true) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            //.UseSqlServer(
            //    connectionString: configuration.GetConnectionString("ConnectionStrings"),
            //        sqlServerOptionsAction: optionsBuilder
            //            => optionsBuilder
            //            .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

            #endregion ============== SQL-SERVER-STRATEGY-2 ==============
        });

        services.AddIdentityCore<AppUser>(options =>
        {
            options.Lockout.AllowedForNewUsers = true;//Cấu hình cho phép khóa tài khoản người dùng mới
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Cấu hình login quá số lần thất bại sẽ bị khóa trong 5 phút
            options.Lockout.MaxFailedAccessAttempts = 5;//Cấu hình số lần đăng nhập thất bại
        })
        .AddRoles<AppRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        //lấy cấu hình password từ file cấu hình
        var passwordOptions = services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<PasswordValidatorOptions>>;

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); //default 5
            options.Lockout.MaxFailedAccessAttempts = 3;//default 5
        });
    }

    public static void AddRepositoryBaseConfiguration(this IServiceCollection services)
    {
        services.AddTransient(typeof(IUnitOfWork), typeof(EFUnitOfWork));
        services.AddTransient(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));

        services.AddTransient(typeof(IUnitOfWorkDbContext<>), typeof(EFUnitOfWorkDbContext<>));
        services.AddTransient(typeof(IRepositoryBaseDBContext<,,>), typeof(RepositoryBaseDbContext<,,>));
    }

    public static void AddInterceptorsPersistence(this IServiceCollection services)
    {
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
    }

    public static OptionsBuilder<MySqlRetryOptions> ConfigureMySqlRetryOptions(this IServiceCollection services, IConfigurationSection section)
    {
        return services
                  .AddOptions<MySqlRetryOptions>()
                  .Bind(section)
                  .ValidateDataAnnotations()
                  .ValidateOnStart();
    }
}
