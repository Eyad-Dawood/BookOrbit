namespace BookOrbit.Application.Common.Interfaces;

public interface IImageService
{
    public Task<Result<string>> GetImageUrlById(Guid Id,CancellationToken token);

}

