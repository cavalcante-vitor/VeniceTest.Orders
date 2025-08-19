using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Venice.Orders.Domain;
using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Infrastructure.DataProviders.Contexts;

[ExcludeFromCodeCoverage]
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<Entity>();
        builder.Ignore<OrderItem>();
        builder.Ignore<Event>();

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);
    }
}