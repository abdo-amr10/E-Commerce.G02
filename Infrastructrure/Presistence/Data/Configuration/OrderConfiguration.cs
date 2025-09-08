using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Order_Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Presistence.Data.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, address => address.WithOwner());
            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(o => o.PaymentStatus).HasConversion
                (
                    s=> s.ToString(),
                    s=> Enum.Parse<OrderPaymentStatus>(s)
                );

            builder.HasOne(o => o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);
            builder.Property(o=> o.SubTotal).HasColumnType("decimal(18,3)");
        }
    }
}
