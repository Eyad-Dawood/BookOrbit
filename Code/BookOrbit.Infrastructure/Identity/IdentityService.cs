namespace BookOrbit.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppUser> userManager) : IIdentityService
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
}
