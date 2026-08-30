using TrainingPlanner.Components;
using TrainingPlanner.Services.Api;
using TrainingPlanner.Services.Contracts;
using TrainingPlanner.Services.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;



var builder = WebApplication.CreateBuilder(args);

if(!IsSecretSet(builder.Configuration)) {
    throw new Exception("user-secrets is not setup correctly");
}

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiBaseUrl = builder.Configuration.GetValue<string>("Api:BaseUrl") ?? "https://localhost:5001/";

builder.Services.AddHttpClient<ITrainingPlannerApiClient, TrainingPlannerApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl, UriKind.Absolute);
});

builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IAgendaService, AgendaService>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear(); 
    options.KnownProxies.Clear();
});

// Configure authentication and related services before building the app
AddAuthentication(builder);

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapGet("/login/github", () => Results.Challenge(
    
    new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = "/" },
    new List<string> { "GitHub" }));
    
app.MapGet("/logout", () => Results.SignOut(
    new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = "/" },
    new List<string> { CookieAuthenticationDefaults.AuthenticationScheme }));

app.Run();
//

static void AddAuthentication(WebApplicationBuilder builder)
{
    // Tilføj den indbyggede state provider til Blazor UI'et
    builder.Services.AddCascadingAuthenticationState();

    builder.Services.AddAuthentication(options => {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "GitHub";
    })
    .AddCookie()
    .AddGitHub(options => {
        options.ClientId = builder.Configuration["GitHub:ClientId"];
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"];
        // GitHub kræver en callback path, standard er /signin-github
        options.CallbackPath = "/signin-github"; 
    });
}

static bool IsSecretSet(IConfiguration configuration)
{
    if (string.IsNullOrEmpty(configuration["GitHub:ClientId"]) || string.IsNullOrEmpty(configuration["GitHub:ClientSecret"])) {
        return false;
    }
    return true;
}