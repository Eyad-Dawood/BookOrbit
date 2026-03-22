namespace BookOrbit.Api.Common.Constatns;
static public class ApiConstants
{
    public const int NormalRateLimiteMaxRequests = 60;
    public const int NormalRateLimitWindowSpanInMinutes = 1;
    public const int NormalRateLimitSegmentPerWindow = 6;
    public const int NormalRateLimitQueueLimit = 10;


    public const int SensistiveRateLimiteMaxRequests = 5;
    public const int SensistiveRateLimitWindowSpanInMinutes = 1;
    public const int SensistiveRateLimitSegmentPerWindow = 2;
    public const int SensistiveRateLimitQueueLimit = 0;


    public const QueueProcessingOrder RateLimitQueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    public const bool RateLimitAutoReplenishment = true;

    public const string SensitiveRateLimmitingPolicyName = "SensitiveRateLimite";
    public const string NormalRateLimitingPolicyName = "NormalRateLimite";


    public const string DefaultOutputCachePolicyName = "DefaultCache";

    public const string StudentImagesUploadFolderPath = "uploads/Students";

}

