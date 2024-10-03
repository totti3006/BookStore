using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infrastructure.EntityConfigurations
{
    internal class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.HasKey(bg => new { bg.GenreId, bg.BookId });

            builder.HasOne(bg => bg.Genre)
                   .WithMany(g => g.BookGenres)
                   .HasForeignKey(bg => bg.GenreId)
                   .IsRequired();

            builder.HasOne(bg => bg.Book)
                   .WithMany(b => b.BookGenres)
                   .HasForeignKey(bg => bg.BookId)
                   .IsRequired();
        }
    }
}
