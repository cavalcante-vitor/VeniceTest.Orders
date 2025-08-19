using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Venice.Orders.Domain.Enums;

namespace Venice.Orders.Infrastructure.DataProviders.Contexts;

[ExcludeFromCodeCoverage]
public sealed class UnitOfWork(
    ApplicationDbContext context, 
    ILogger<UnitOfWork> logger) : IUnitOfWork, IDisposable
{
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        (await context.SaveChangesAsync(cancellationToken)) > 0;
    
    public async Task ExecuteTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await operation();
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, ExceptionsMessages.DatabaseException.GetDescription());
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public void Dispose()
    {
        context.Dispose();
    }
}