
using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");
        builder.HasKey(s => s.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
        builder.Property(s => s.CustomerId).IsRequired();
        builder.Property(s => s.CustomerName).IsRequired().HasMaxLength(255);
        builder.Property(s => s.BranchId).IsRequired().HasMaxLength(100);
        builder.Property(s => s.BranchName).IsRequired().HasMaxLength(255);
        builder.Property(s => s.TotalAmount).HasColumnType("decimal(18,2)");
        builder.HasMany<SaleItem>("_items").WithOne().HasForeignKey(i => i.SaleId);
        builder.Navigation("_items").UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(s => s.Customer)
           .WithMany(c => c.Sales)
           .HasForeignKey(s => s.CustomerId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<SaleItem>("_items")
          .WithOne()
          .HasForeignKey(i => i.SaleId)
          .OnDelete(DeleteBehavior.Cascade); 

        builder.Navigation("_items").UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}
