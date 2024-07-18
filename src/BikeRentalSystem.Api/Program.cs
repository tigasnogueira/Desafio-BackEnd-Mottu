using Asp.Versioning.ApiExplorer;
using BikeRentalSystem.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

var apiSettings = new ApiSettings();
apiSettings.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

apiSettings.ConfigurePipeline(app, app.Environment, apiVersionDescriptionProvider);

app.Run();
