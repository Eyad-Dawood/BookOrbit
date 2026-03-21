

using BookOrbit.Application.Common.Constants;

namespace BookOrbit.Infrastructure;
static public class DependencyInjection
{
    static public IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        return services
            .AddDbContext(configuration)
            .AddIdentity()
            .AddInfrastrucureServices()
            .AddPolicies();
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
        services.AddTransient<IAppCache, AppCache>();
        services.AddScoped<AppDbContextInitialiser>();
        return services;
    }
    static private IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler,ActiveUserHandler>();
        services.AddScoped<IAuthorizationHandler,AdminOnlyHandler>();
        services.AddScoped<IAuthorizationHandler,StudentOwnerShipHandler>();


        services.AddAuthorizationBuilder()

            .AddPolicy(PoliciesNames.ActiveUsersPolicy, policy =>
            policy.Requirements.Add(new ActiveUserRequirement()))

            .AddPolicy(PoliciesNames.AdminOnlyPolicy, policy => {
            policy.Requirements.Add(new ActiveUserRequirement());
            policy.Requirements.Add(new AdminOnlyRequirement());
            })

            .AddPolicy(PoliciesNames.StudentOwnershipPolicy,policy =>
            {
                policy.Requirements.Add(new ActiveUserRequirement());
                policy.Requirements.Add(new StudentOwnerShipRequirement());
            })

            ;

        return services;
    }

}