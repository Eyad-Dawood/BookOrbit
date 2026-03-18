namespace BookOrbit.Application.Common.Interfaces;
public interface IIdentityService
{
    Task<Result<AppUserDto>> AuthenticateAsync(string email,string password,CancellationToken ct);
    Task<Result<AppUserDto>> GetUserByIdAsync(string id, CancellationToken ct);
}
