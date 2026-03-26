
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

        builder.Property(b => b.Category)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(b => b.CoverImageFileName)
            .HasMaxLength(255)
            .IsUnicode(false)
            .IsRequired();


        builder.OwnsOne(b => b.Title, t =>
        {
            t.Property(x => x.Value)
             .HasColumnName("Title")
             .HasMaxLength(BookValidationConstants.TitleMaxLength)
             .IsRequired();

            t.HasIndex(x => x.Value);
        });


        builder.OwnsOne(b => b.ISBN, i =>
        {
            i.WithOwner();

            i.Property(x => x.Value)
             .HasColumnName("ISBN")
             .HasMaxLength(BookValidationConstants.ISBNMaxLength)
             .IsUnicode(false)
             .IsRequired();

            i.HasIndex(x => x.Value).IsUnique();
        });


        builder.OwnsOne(b => b.Publisher, p =>
        {
            p.Property(x => x.Value)
             .HasColumnName("Publisher")
             .HasMaxLength(BookValidationConstants.PublisherMaxLength)
             .IsRequired();
        });


        builder.OwnsOne(b => b.Author, a =>
        {
            a.Property(x => x.Value)
             .HasColumnName("Author")
             .HasMaxLength(BookValidationConstants.AuthorMaxLength)
             .IsRequired();
        });
    }
}