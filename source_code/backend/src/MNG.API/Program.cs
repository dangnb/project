using MNG.API.DependencyInjection.Extensions;
using MNG.API.Middleware;
using MNG.Application.DependencyInjection.Extensions;
using MNG.Persistence.DependencyInjection.Extentions;
using MNG.Persistence.DependencyInjection.Options;
using MNG.Infrastructure.DependencyInjection.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();
builder.Host.UseSerilog();

builder.Services.AddJwtAuthentication(builder.Configuration);

//Add configuration
builder.Services.ConfigureMySqlRetryOptions(builder.Configuration.GetSection(nameof(MySqlRetryOptions)));
builder.Services.AddConfigureMediatR();
builder.Services.AddSqlConfiguration();
builder.Services.AddInterceptorsPersistence();
builder.Services.AddRepositoryBaseConfiguration();
builder.Services.AddConfigureAutoMapper();
builder.Services.AddInfrastructureService();
//builder.Services.AddInfrastructureDapper();

builder.Services.AddSwagger();

builder.Services.AddControllers()
    .AddApplicationPart(MNG.API.AssemblyReference.assembly);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

//builder.Services
//        .AddSwaggerGenNewtonsoftSupport()
//       .AddFluentValidationRulesToSwagger()
//        .AddEndpointsApiExplorer()
//        .AddSwagger();

builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
{
    app.ConfigureSwagger();
}

try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}


//app.Run(); 
