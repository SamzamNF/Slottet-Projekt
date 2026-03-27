using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Slottet.Infrastructure.Persistence;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.

// Appsettings configuration

// Api appsettings configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// EF DbContext

/*builder.Services.AddDbContext<EFContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); ------------slet kommentar her når klar*/

// Implementations from Infrastructure based on their interface in Application layer

// Application services (Business logic)


// Cors with frontend URL

builder.Services.AddCors(options =>
{
    options.AddPolicy("SlottetFrontEnd", policy =>
    {
        policy.WithOrigins("https://localhost:7189")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Og efter builder.Build():


builder.Services.AddControllers();
builder.Services.AddAuthorization();



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseCors("SlottetFrontEnd");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
