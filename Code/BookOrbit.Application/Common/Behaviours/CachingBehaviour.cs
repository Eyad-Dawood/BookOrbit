namespace BookOrbit.Application.Common.Behaviours;
public class CachingBehaviour<TRequest, TResponse>(
    ILogger<CachingBehaviour<TRequest,TResponse>> logger,
    IAppCache appCache)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        logger.LogInformation("Handling Cache for {RequestName}", typeof(TRequest).Name);

        
        var result = await appCache.GetOrCreateAsync<TResponse>(
            request.CacheKey,
            async cancelToken =>
            {
                var response = await next(cancelToken);

                if (response is IResult res && !res.IsSuccess)
                {
                    throw new InvalidOperationException("Do not cache failed result");
                }

                return response;
            },
            request.Expiration,
            tags: request.Tags, 
            cancellationToken: ct);

        return result;
    }
}