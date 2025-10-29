using Microsoft.EntityFrameworkCore;

namespace MNG.Domain.Abstractions;
public interface IUnitOfWorkDbContext<TContext> : IAsyncDisposable
    where TContext : DbContext
{
    /// <summary>
    /// Call save change from db context
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    DbContext GetDbContext();
}
