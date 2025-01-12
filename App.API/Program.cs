using App.API.Filters;
using App.Persistence.Extensions;
using App.Application.Extensions;
using App.API.ExceptionHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add<FluentValidationFilter>();
    opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(NotFoundFilter<,>));
builder.Services.AddExceptionHandler<CriticalExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
var app = builder.Build();

app.UseExceptionHandler(p => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
