using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using URL_Shortener.Extensions;
using URLShortener.Application.Extensions;
using URLShortener.Application.Mappings;
using URLShortener.infrastructure.Contexts;
using URLShortener.infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();
//builder.Services.AddCustomSwagger();
builder.Services.AddControllers();
// Registrar IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

//configure db Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//automapper
builder.Services.AddAutoMapper(typeof(UserProfile));
// Configurar Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "URLShortener API",
        Version = "v1",
        Description = "API para acortar URLs",
        Contact = new OpenApiContact
        {
            Name = "Tu Nombre",
            Email = "tu-email@example.com"
        }
    });
});
// Abstraemos configuración de rate limiting
builder.Services.AddAppRateLimiting(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "URLShortener API v1");
    });
}


app.UseHttpsRedirection();
app.UseRouting();
// Activamos rate limiting
app.UseAppRateLimiting();
app.UseAuthorization();


app.MapControllers();

app.Run();
