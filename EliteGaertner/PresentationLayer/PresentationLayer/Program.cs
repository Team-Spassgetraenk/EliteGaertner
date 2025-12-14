using PresentationLayer.Components;
using AppLogic.Interfaces;
using AppLogic.Services;
using PresentationLayer.Components.Pages.Register;

var builder = WebApplication.CreateBuilder(args);

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