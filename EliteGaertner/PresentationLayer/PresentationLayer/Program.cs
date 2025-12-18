using PresentationLayer.Components;
using DataManagement;
using Microsoft.EntityFrameworkCore;
using AppLogic.Interfaces;
using AppLogic.Services;
using PresentationLayer.Components.Pages.Register;

var builder = WebApplication.CreateBuilder(args);

//ConnectionString und Initialisierung für DbContext
var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? "Host=localhost;Port=5432;Database=elitegaertner;Username=postgres;Password=postgres";

builder.Services.AddDbContext<EliteGaertnerDbContext>(options =>
    options.UseNpgsql(connectionString));
//ÜBERGABE DER DBCONTEXT AN DIE MANAGEMENTDDBS FEHLT!!!!


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Blazor Server interaktiv

// Registrierungsstatus **vor Build registrieren**
builder.Services.AddSingleton<UserRegistrationState>();

// UploadService registrieren
builder.Services.AddScoped<IUploadService, UploadServiceImpl>();

// ProfileMgm registrieren
builder.Services.AddScoped<IProfileMgm, ProfileMgm>();

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

// Map Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Für Server-Komponenten

app.Run();