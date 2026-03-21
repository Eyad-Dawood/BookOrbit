namespace BookOrbit.Infrastructure.Data;

public class AppDbContextInitialiser(
    ILogger<AppDbContextInitialiser> logger,
    UserManager<AppUser> userManager,
    AppDbContext context,
    RoleManager<IdentityRole> roleManager)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        #region Roles

        var studentRoleName = nameof(IdentityRoles.student);
        var adminRoleName = nameof(IdentityRoles.admin);

        if (!await roleManager.RoleExistsAsync(studentRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(studentRoleName));
            logger.LogInformation("Student role created");
        }

        if (!await roleManager.RoleExistsAsync(adminRoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            logger.LogInformation("Admin role created");
        }

        #endregion

        #region Students

        var students = new List<AppUser>
        {
            new()
            {
                Email = "student1@std.mans.edu.eg",
                UserName = "student1@std.mans.edu.eg",
                EmailConfirmed = true
            },
            new()
            {
                Email = "student2@std.mans.edu.eg",
                UserName = "student2@std.mans.edu.eg",
                EmailConfirmed = true
            },
            new()
            {
                Email = "student3@std.mans.edu.eg",
                UserName = "student3@std.mans.edu.eg",
                EmailConfirmed = true
            },
            new()
            {
                Email = "student4@std.mans.edu.eg",
                UserName = "student4@std.mans.edu.eg",
                EmailConfirmed = true
            },
            new()
            {
                Email = "student5@std.mans.edu.eg",
                UserName = "student5@std.mans.edu.eg",
                EmailConfirmed = true
            }
        };

        string studentPassword = "sa123456";

        int index = 1;

        foreach (var studentUser in students)
        {
            var user = await CreateUserIfNotExistsAsync(studentUser, studentPassword, studentRoleName);

            if (user is null)
                continue;


            await CreateStudentIfNotExistsAsync(user, index);
            index++;
        }

        #endregion

        #region Admin

        var admin = new AppUser
        {
            Email = "admin@bookorbit.com",
            UserName = "admin@bookorbit.com",
            EmailConfirmed = true
        };

        string adminPassword = "Admin@123456";

        await CreateUserIfNotExistsAsync(admin, adminPassword, adminRoleName);

        #endregion
    }

    #region Helpers

    private async Task CreateStudentIfNotExistsAsync(AppUser user,int index)
    {
        // check if student already exists
        if (await context.Students.AnyAsync(s => s.UserId == user.Id))
            return;

        var nameResult = StudentName.Create($"Student {index.ToWords(new CultureInfo("en"))} Name");
        var emailResult = UniversityMail.Create(user.Email!);
        string personalPhoto =$"{Guid.NewGuid()}.png";
        var telegramUserIdResult = TelegramUserId.Create($"student{index}");
        var phoneNumberResult = PhoneNumber.Create($"0109690981{index}");

        if (nameResult.IsFailure)
        {
            logger.LogError("Failed to create student value objects for user {Email} Errro {Error}", user.Email,nameResult.Errors);
            return;
        }

        if (emailResult.IsFailure)
        {
            logger.LogError("Failed to create student value objects for user {Email} Errro {Error}", user.Email, emailResult.Errors);
            return;
        }


        if (telegramUserIdResult.IsFailure)
        {
            logger.LogError("Failed to create student value objects for user {Email} Errro {Error}", user.Email, telegramUserIdResult.Errors);
            return;
        }

        if (phoneNumberResult.IsFailure)
        {
            logger.LogError("Failed to create student value objects for user {Email} Errro {Error}", user.Email, phoneNumberResult.Errors);
            return;
        }


        var studentResult = Student.Create(
            Guid.NewGuid(),
            nameResult.Value,
            emailResult.Value,
            personalPhoto,
            user.Id,
            phoneNumber: phoneNumberResult.Value,
            telegramUserId: telegramUserIdResult.Value
        );

        if (studentResult.IsFailure)
        {
            logger.LogError("Failed to create student entity for user {Email}", user.Email);
            return;
        }

        context.Students.Add(studentResult.Value);
        await context.SaveChangesAsync();

        logger.LogInformation("Student created for user {Email}", user.Email);
    }

    private async Task<AppUser?> CreateUserIfNotExistsAsync(AppUser user, string password, string role)
    {
        var existingUser = await userManager.FindByEmailAsync(user.Email!);

        if (existingUser != null)
        {
            logger.LogInformation("User {Email} already exists", user.Email);
            return existingUser;
        }

        user.Id = Guid.NewGuid().ToString();

        var createResult = await userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            logger.LogError("Failed to create user {Email}: {Errors}",
                user.Email,
                string.Join(", ", createResult.Errors.Select(e => e.Description)));
            return null;
        }

        var roleResult = await userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            logger.LogError("Failed to assign role {Role} to user {Email}: {Errors}",
                role,
                user.Email,
                string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        logger.LogInformation("User {Email} created with role {Role}", user.Email, role);

        return user;
    }
    #endregion
}

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<AppDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}