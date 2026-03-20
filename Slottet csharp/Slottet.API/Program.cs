using Microsoft.EntityFrameworkCore;
using Slottet.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.

// Appsettings configuration

// EF DbContext

builder.Services.AddDbContext<EFContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 

// Implementations from Infrastructure based on their interface in Application layer

// Application services (Business logic)


builder.Services.AddControllers();



// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
