using HotelDirectory.Hotel.Service.Application.Extension;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Context;
using HotelDirectory.Shared.ElasticSearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#if DEBUG
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
#endif
#if RELEASE
Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
#endif

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddSingleton<IConfiguration>(configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    //AddSwaggerXml(options);
    options.UseInlineDefinitionsForEnums();
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel.Application",
        Version = "v1",
        Description = "Hotel Servisi"
    });
});


builder.Services.AddDbContext<HotelDbContext>(options =>
{
    options.UseNpgsql(configuration.GetSection("ConnectionStrings:HotelDbConnection").Value);
});

ApplicationExtension.RegisterService(builder.Services, configuration);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
