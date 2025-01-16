using App.API.Extensions;
using App.API.Extensons;
using App.Application.Contracts.Caching;
using App.Application.Extensions;
using App.Caching;
using App.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllerWithFiltersExt().AddSwaggerGenExt().AddExceptionHandler().AddCachingExt();
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);
builder.Services.AddSingleton<ICacheService, CacheService>();

var app = builder.Build();

app.ConfigurePipelineExt();

app.MapControllers();

app.Run();
