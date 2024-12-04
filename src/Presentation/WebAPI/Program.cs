using Domain;
using Persistence;
using Application;
using Infrastructure;
using WebAPI.Extensions;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); 
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDomain(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

string templateDirectory = Path.Combine(builder.Environment.ContentRootPath, "StoredFiles");
builder.Services.AddSingleton<ITemplateProvider>(new TemplateProvider(templateDirectory));

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

builder.Services.ConfigureCors(builder.Configuration, MyAllowSpecificOrigins);
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>((httpContext) =>
    {
        var route = httpContext.GetEndpoint()?.DisplayName ?? "default";
        var partitionKey = $"{route}:{httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString()}";

        return RateLimitPartition.GetFixedWindowLimiter(
               partitionKey: partitionKey,
               factory: partition => new FixedWindowRateLimiterOptions
               {
                    AutoReplenishment = true,
                    PermitLimit = 10,
                    Window = TimeSpan.FromMinutes(1)
               });
    });

    options.AddFixedWindowLimiter("Fixed", config =>
    {
        config.PermitLimit = 5;
        config.Window = TimeSpan.FromSeconds(10);
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 2; 
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRateLimiter();

app.UseStaticFiles();

app.UseCors(MyAllowSpecificOrigins);

app.UseExceptionHandler();

app.MapHealthChecks("/healthz");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
