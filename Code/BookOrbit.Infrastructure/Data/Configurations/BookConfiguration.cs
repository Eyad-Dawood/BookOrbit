
namespace BookOrbit.Infrastructure.Data.Configurations;
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ConfigureAuditable();

        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever();

        builder.Property(b => b.Title)
            .HasConversion(
                title => title.Value,
                value => BookTitle.Create(value).Value)
            .HasMaxLength(BookValidationConstants.TitleMaxLength)
            .IsRequired();

        builder.Property(b => b.ISBN)
            .HasConversion(
                isbn => isbn.Value,
                value => ISBN.Create(value).Value)
            .HasMaxLength(BookValidationConstants.ISBNMaxLength)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(b => b.Publisher)
            .HasConversion(
                publisher => publisher.Value,
                value => BookPublisher.Create(value).Value)
            .HasMaxLength(BookValidationConstants.PublisherMaxLength)
            .IsRequired();

        builder.Property(b => b.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(b => b.Author)
            .HasConversion(
                author => author.Value,
                value => BookAuthor.Create(value).Value)
            .HasMaxLength(BookValidationConstants.AuthorMaxLength)
            .IsRequired();

        builder.Property(b => b.CoverImageFileName)
            .HasMaxLength(255)
            .IsUnicode(false)
            .IsRequired();

        builder.HasIndex(b => b.ISBN)
            .IsUnique();

        builder.HasIndex(b => b.Title);
    }
}