using Serilog.Enrichers.Span;

var builder = WebApplication.CreateBuilder(args);

builder.Services.
    AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, services, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)

        .Enrich.FromLogContext()
        .Enrich.WithSpan();
});

var app = builder.Build();

app.UseCoreMiddlewares(builder.Configuration);
app.MapControllers();


app.Run();