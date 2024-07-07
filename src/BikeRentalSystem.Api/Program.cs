using BikeRentalSystem.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

var apiSettings = new ApiSettings();
apiSettings.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

apiSettings.ConfigurePipeline(app, app.Environment);

app.Run();
