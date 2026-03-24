using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Slottet.Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Microsoft Entra Authentication (Erstatter alt det gamle Identity lort)
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi(new string[] { "api://bd20e841-d13e-49bc-98be-6e4c60872fd2/access_as_user" })
    .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();
builder.Services.AddCascadingAuthenticationState();

//Blazor Services
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// API Connection (HTTP Client)
builder.Services.AddHttpClient("MyAPI", client => client.BaseAddress = new Uri("https://localhost:7189"));
builder.Services.AddScoped<ApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();