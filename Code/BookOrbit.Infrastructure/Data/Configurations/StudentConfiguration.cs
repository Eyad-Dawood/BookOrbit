namespace BookOrbit.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ConfigureAuditable();

        builder.ToTable("Students");

        builder.HasKey(s => s.Id).IsClustered(false);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .HasConversion(
                name => name.Value,
                value => StudentName.Create(value).Value)
            .HasMaxLength(StudentValidationConstants.NameMaxLength)
            .IsRequired();

        builder.Property(s => s.PhoneNumber)
            .HasConversion(
                phoneNumber => phoneNumber == null ? null : phoneNumber.Value,
                value => string.IsNullOrWhiteSpace(value) ? null : PhoneNumber.Create(value).Value)
            .HasMaxLength(PhoneNumberConstants.MaxLength)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(s => s.TelegramUserId)
            .HasConversion(
                telegramUserId => telegramUserId == null ? null : telegramUserId.Value,
                value => string.IsNullOrWhiteSpace(value) ? null : TelegramUserId.Create(value).Value)
            .HasMaxLength(TelegramUserIdConstants.MaxLength)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(s => s.UniversityMail)
            .HasConversion(
                universityMail => universityMail.Value,
                value => UniversityMail.Create(value).Value)
            .HasMaxLength(UniversityMailConstants.MaxLength)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(s => s.PersonalPhotoUrl)
            .HasConversion(
                url => url.Value,
                value => Url.Create(value).Value)
            .HasMaxLength(2048)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(s => s.Points)
            .HasDefaultValue(0)
            .IsRequired();

        builder.Property(s => s.JoinDateUtc)
            .HasColumnType("datetimeoffset")
            .IsRequired(false);

        builder.Property(s => s.State)
           .HasConversion<string>()
           .HasMaxLength(20)
           .IsRequired();

        builder.HasIndex(s=>s.UniversityMail)
            .IsUnique();
    }
}
