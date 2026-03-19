
namespace BookOrbit.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppUser> userManager,
    ILogger<IdentityService> logger,
    IMaskingService maskingService) : IIdentityService
{
    public async Task<Result<AppUserDto>> AuthenticateAsync(string email, string password, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
            return InfrastrucureIdentityErrors.UserNotFoundByEmail;

        if (!user.EmailConfirmed)
            return InfrastrucureIdentityErrors.EmailNotConfirmed;

        if (!await userManager.CheckPasswordAsync(user, password))
            return InfrastrucureIdentityErrors.InvalidLoginAttempt;


        return new AppUserDto(
            user.Id,
            user.Email!,
            await userManager.GetRolesAsync(user),
            await userManager.GetClaimsAsync(user));
    }

    public async Task<Result<string>> CreateStudent(string email, string password,CancellationToken ct)
    {
        var user = new AppUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false,
        };

        var createResult = await userManager.CreateAsync(user, password);

        if (!createResult.Succeeded)
        {
            logger.LogError(
                "Failed to create user with email {Email}. Errors: {Errors}",
                maskingService.MaskEmail(email),
                string.Join(" | ", createResult.Errors.Select(e => e.Description)));

            return InfrastrucureIdentityErrors.UserCreationFaild;
        }

        string role = nameof(IdentityRoles.student);
        var roleResult = await userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);

            logger.LogError(
                "Failed to add user with email {Email} to role {Role}. Errors: {Errors}",
                maskingService.MaskEmail(email),
                role,
                string.Join(" | ", roleResult.Errors.Select(e => e.Description)));

            return InfrastrucureIdentityErrors.UserCreationFaild;
        }

        return user.Id;
    }

    public async Task<Result<Deleted>> DeleteUserByIdAsync(string userId, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Deleted;

        var deleteResult = await userManager.DeleteAsync(user);

        if (!deleteResult.Succeeded)
        {
            logger.LogError(
                "Failed to delete user with ID {UserId}. Errors: {Errors}",
                userId,
                string.Join(" | ", deleteResult.Errors.Select(e => e.Description)));

            return InfrastrucureIdentityErrors.UserDeletionFailed;
        }
        return Result.Deleted;
    }


    public async Task<Result<AppUserDto>> GetUserByIdAsync(string id, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(id);

        if (user is null)
            return InfrastrucureIdentityErrors.UserNotFoundById;

        return new AppUserDto(
            user.Id,
            user.Email!,
            await userManager.GetRolesAsync(user),
            await userManager.GetClaimsAsync(user));
    }

    public async Task<bool> UserEmailExists(string email, CancellationToken ct) =>
         await userManager.Users.AnyAsync(u => u.Email == email, ct);

}