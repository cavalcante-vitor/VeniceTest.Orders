using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Venice.Orders.Domain.Entities;

namespace Venice.Orders.Infrastructure.DataProviders.Contexts.ModelBuilderConfigurations;

public class OrderModelBuilderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> entity)
    {
        entity.ToTable("Orders");

        entity.HasKey(e => e.Id)
            .HasName("PK_Orders");
        
        entity.Property(e => e.Id)
            .IsRequired()
            .HasMaxLength(36)
            .HasColumnType("VARCHAR(36)")
            .ValueGeneratedNever();
        
        entity.Property(e => e.Date)
            .IsRequired()
            .HasColumnType("datetime2");
        
        entity.Property(e => e.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasColumnType("INT");
        
        entity.OwnsOne(o => o.Customer, owned =>
        {
            owned.Property(c => c.Id).HasColumnName("CustomerId").HasMaxLength(36).IsRequired();
            owned.Property(c => c.Name).HasColumnName("CustomerName").HasMaxLength(200);
            owned.Property(c => c.Email).HasColumnName("CustomerEmail").HasMaxLength(200).IsRequired();
            owned.Property(c => c.PhoneNumber).HasColumnName("CustomerPhoneNumber").HasMaxLength(20);
        });
        
        entity.Navigation(o => o.Customer).IsRequired();

        entity.Ignore(o => o.Items);
        entity.Ignore("_orderItems");
    }
}