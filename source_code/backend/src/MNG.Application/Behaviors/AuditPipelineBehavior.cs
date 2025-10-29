using MNG.Domain.Abstractions;
using MNG.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MNG.Domain.Abstractions.Entities;

namespace MNG.Application.Behaviors;
public sealed class AuditPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWorkDbContext<ApplicationDbContext> _unitOfWork; // SQL-SERVER-STRATEGY-2
    private readonly IHttpContextAccessor _httpContextAccessor;
    //private readonly ApplicationDbContext _context; // SQL-SERVER-STRATEGY-1

    public AuditPipelineBehavior(IUnitOfWorkDbContext<ApplicationDbContext> unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        //_context = context;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!IsCommand()) // In case TRequest is QueryRequest just ignore
            return await next();

        #region ============== SQL-SERVER-STRATEGY-1 ==============

        //// Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //// https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
        var strategy = _unitOfWork.GetDbContext().Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _unitOfWork.GetDbContext().Database.BeginTransactionAsync();
            {
                var response = await next();


                foreach (var entry in _unitOfWork.GetDbContext().ChangeTracker.Entries<IUserTracking>())
                {
                    var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedBy = userId;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastModifiedBy = userId;
                    }
                }

                foreach (var entry in _unitOfWork.GetDbContext().ChangeTracker.Entries<IDateTracking>())
                {
                    var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                    }
                }

                await _unitOfWork.GetDbContext().SaveChangesAsync();
                await transaction.CommitAsync();
                return response;
            }
        });
        #endregion ============== SQL-SERVER-STRATEGY-1 ==============

        #region ============== SQL-SERVER-STRATEGY-2 ==============

        //IMPORTANT: passing "TransactionScopeAsyncFlowOption.Enabled" to the TransactionScope constructor. This is necessary to be able to use it with async/await.
        //using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //{
        //    var response = await next();
        //    await _unitOfWork.SaveChangesAsync(cancellationToken);
        //    transaction.Complete();
        //    await _unitOfWork.DisposeAsync();
        //    return response;
        //}
        #endregion ============== SQL-SERVER-STRATEGY-2 ==============

    }

    private bool IsCommand()
        => typeof(TRequest).Name.EndsWith("Command");
}
