namespace BookOrbit.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ConfigureAuditable();

        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

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
            .HasMaxLength(PhoneNumberValidationConstants.MaxLength)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(s => s.TelegramUserId)
            .HasConversion(
                telegramUserId => telegramUserId == null ? null : telegramUserId.Value,
                value => string.IsNullOrWhiteSpace(value) ? null : TelegramUserId.Create(value).Value)
            .HasMaxLength(TelegramUserIdValidationConstants.MaxLength)
            .IsUnicode(false)
            .IsRequired(false);

        builder.Property(s => s.UniversityMail)
            .HasConversion(
                universityMail => universityMail.Value,
                value => UniversityMail.Create(value).Value)
            .HasMaxLength(UniversityMailValidationConstants.MaxLength)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(s => s.PersonalPhotoFileName)
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

        builder.HasIndex(s => s.UserId)
            .IsUnique();

        builder.HasOne<AppUser>()
            .WithOne()
            .HasForeignKey<Student>(x => x.UserId)
            .HasPrincipalKey<AppUser>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
