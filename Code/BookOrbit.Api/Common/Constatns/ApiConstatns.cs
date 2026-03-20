namespace BookOrbit.Api.Common.Constatns;
static public class ApiConstatns
{
    public const int RateLimiteMaxRequests = 100;
    public const int RateLimitWindowSpanInMinutes = 1;
    public const int RateLimitSegmentPerWindow = 6;
    public const int RateLimitQueueLimit = 20;

    public const string DefaultOutputCachePolicyName = "DefaultCache";

}

