using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.EntityConfigurations
{
    internal class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.Description)
                   .HasMaxLength(1000);

            builder.Property(b => b.Price)
                   .IsRequired()
                   .HasColumnType("Money");

            builder.HasMany(b => b.Carts)
                   .WithOne(c => c.Book)
                   .HasForeignKey(c => c.BookId);

            builder.HasMany(b => b.FavouriteBooks)
                   .WithOne(fb => fb.Book)
                   .HasForeignKey(fb => fb.BookId);

            builder.HasMany(b => b.Reviews)
                   .WithOne(r => r.Book)
                   .HasForeignKey(r => r.BookId);
        }
    }
}
