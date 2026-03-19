
namespace BookOrbit.Infrastructure;
static public class DependencyInjection
{
    static public IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        return services
            .AddDbContext(configuration)
            .AddAuthentication(configuration)
            .AddIdentity()
            .AddInfrastrucureServices()
            .AddHybridCaching()
            .AddAppOutputCaching();
    }
    static private IServiceCollection AddDbContext(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());


        return services;
    }
    static private IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options=>
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),
            };
        });

        return services;
    }
    static private IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services
        .AddIdentityCore<AppUser>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredUniqueChars = 4;
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
    static private IServiceCollection AddInfrastrucureServices(this IServiceCollection services)
    {
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ITokenProvider, TokenProvider>();
        services.AddTransient<IMaskingService, MaskingService>();
        return services;
    }
    static private IServiceCollection AddHybridCaching(this IServiceCollection services)
    {
        services.AddHybridCache(options => options.DefaultEntryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(DefaultValues.RemoteCachExpirationInMinutes), //Remote
            LocalCacheExpiration = TimeSpan.FromSeconds(DefaultValues.LocalCachExpirationInSeconds), // Local
        });

        return services;
    }
    public static IServiceCollection AddAppOutputCaching(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.SizeLimit = 100 * 1024 * 1024; // 100 mb
            options.AddBasePolicy(policy =>
                policy.Expire(TimeSpan.FromSeconds(DefaultValues.OutputCachExpirationInSeconds)));
        });

        return services;
    }
}