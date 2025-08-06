using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("OrderProducts");

            builder.HasKey(op => op.Id);

            builder.Property(op => op.OrderId)
                .IsRequired();

            builder.Property(op => op.ProductId)
                .IsRequired();

            builder.Property(op => op.Quantidade)
                .IsRequired();

            builder.Property(op => op.PrecoUnitario)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(op => op.DataCriacao)
                .IsRequired();

            builder.HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 