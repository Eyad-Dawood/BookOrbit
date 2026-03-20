namespace BookOrbit.Application.Common.Interfaces;
public interface IIdentityService
{
    Task<Result<AppUserDto>> AuthenticateAsync(string email,string password,CancellationToken ct);
    Task<Result<AppUserDto>> GetUserByIdAsync(string id, CancellationToken ct);
    Task<Result<string>> CreateStudent(string email,string password,CancellationToken ct);
    Task<Result<Deleted>> DeleteUserByIdAsync(string userId, CancellationToken ct);
    Task<bool> UserEmailExists(string email,CancellationToken ct);
    Task<string?> GetUserNameAsync(string userId,CancellationToken ct);
}