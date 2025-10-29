using Microsoft.EntityFrameworkCore;
using MNG.Domain.Abstractions;

namespace MNG.Persistence;
public class EFUnitOfWorkDbContext<TContext> : IUnitOfWorkDbContext<TContext>
    where TContext : DbContext
{
    private readonly TContext _context;

    public EFUnitOfWorkDbContext(TContext context)
        => _context = context;

    public DbContext GetDbContext()
    {
        return _context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync();

    async ValueTask IAsyncDisposable.DisposeAsync()
        => await _context.DisposeAsync();
}
