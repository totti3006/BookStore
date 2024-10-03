using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.EntityConfigurations
{
    internal class OrderBookConfiguration : IEntityTypeConfiguration<OrderBook>
    {
        public void Configure(EntityTypeBuilder<OrderBook> builder)
        {
            builder.HasKey(ob => new { ob.OrderId, ob.BookId });

            builder.HasOne(ob => ob.Order)
                   .WithMany(o => o.OrderBooks)
                   .HasForeignKey(ob => ob.OrderId)
                   .IsRequired();

            builder.HasOne(ob => ob.Book)
                   .WithMany(b => b.OrderBooks)
                   .HasForeignKey(ob => ob.BookId)
                   .IsRequired();
        }
    }
}
