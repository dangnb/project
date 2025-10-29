using Microsoft.EntityFrameworkCore;

namespace MNG.Domain.Abstractions;
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Call save change from db context
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    DbContext GetDbContext();
}
