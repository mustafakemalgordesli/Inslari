using Domain;
using Persistence;
using Application;
using Infrastructure;
using WebAPI.Extensions;
using System.Text.Json.Serialization;
using Domain.Resources;
using Microsoft.Extensions.Localization;
using Domain.Abstractions;

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
builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureCors(builder.Configuration, MyAllowSpecificOrigins);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureRateLimiter();
builder.Services.ConfigureCompression();
builder.Services.ConfigureLocalization();
builder.Services.ConfigureMassTransit(builder.Configuration);
builder.Services.ConfigureApiVersioning();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRequestLocalization(LocalizationConfig.GetLocalizationOptions());

var localizer = app.Services.GetRequiredService<IStringLocalizer<SharedResource>>();
StringLocalizer.Configure(localizer);

app.UseResponseCompression();

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
