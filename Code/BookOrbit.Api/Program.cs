var builder = WebApplication.CreateBuilder(args);

builder.Services.
    AddPresentation(builder.Configuration)
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, loggerConfig) =>
loggerConfig.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseCoreMiddlewares(builder.Configuration);
app.MapControllers();


app.Run();