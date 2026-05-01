using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Slottet.Infrastructure.Persistence;
using Microsoft.Identity.Web;
using Slottet.Application.Interfaces;
using Slottet.Infrastructure.Repositories;
using Slottet.Application.BusinessLogic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container.

// Appsettings configuration

// Api appsettings configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// EF DbContext

builder.Services.AddDbContext<EFContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Implementations from Infrastructure based on their interface in Application layer
builder.Services.AddScoped<IUnitOfWork, EFContext>();

builder.Services.AddScoped<IStaffRepository, EfStaffRepository>();
builder.Services.AddScoped<IPostItRepository, EfPostItRepository>();
builder.Services.AddScoped<IResidentRepository, EfResidentRepository>();
builder.Services.AddScoped<IRoleRepository, EfRoleRepository>();
builder.Services.AddScoped<IDepartmentRepository, EfDepartmentRepository>();
builder.Services.AddScoped<IResponsibilityAreaRepository, EfResponsibilityAreaRepository>();

// Application services (Business logic/Services)
builder.Services.AddScoped<StaffService>();
builder.Services.AddScoped<PostItService>();
builder.Services.AddScoped<ResidentService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<ResponsibilityAreaService>();

// Register cleanup background worker
//builder.Services.AddHostedService<Slottet.Application.BackgroundServices.ResidentRetentionService>();

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


// Middleware and controllers


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