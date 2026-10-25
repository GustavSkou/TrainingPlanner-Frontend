using TrainingPlanner.Components;
using TrainingPlanner.Services.Api;
using TrainingPlanner.Services.Contracts;
using TrainingPlanner.Services.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using TrainingPlanner.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiBaseUrl = builder.Configuration["Api:BaseUrl"] ?? "https://localhost:5001/";

builder.Services.AddHttpClient<ITrainingPlannerApiClient, TrainingPlannerApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl, UriKind.Absolute);
    client.DefaultRequestHeaders.Add("API-KEY", builder.Configuration["API-KEY"]);
});

builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IAgendaService, AgendaService>();
builder.Services.AddScoped<IUserService, UserService>();

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

app.MapGet("/", () => Results.Redirect("/calendar"));

app.MapGet("/login/github", () => Results.Challenge(
    new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = "/calendar" },
    new List<string> { "GitHub" }));

app.MapGet("/logout", () => Results.SignOut(
    new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = "/login" },
    new List<string> { CookieAuthenticationDefaults.AuthenticationScheme }));

/*app.MapGet("/register/github", (string firstName, string lastName) =>
{
    if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
    {
        return Results.Redirect("/register");
    }

    var properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
    {
        RedirectUri = "/login"
    };
    properties.Items["registration:flow"] = "true";
    properties.Items["registration:firstName"] = firstName.Trim();
    properties.Items["registration:lastName"] = lastName.Trim();

    return Results.Challenge(properties, new List<string> { "GitHub" });
});*/

app.Run();
//

static void AddAuthentication(WebApplicationBuilder builder)
{
    // Tilføj den indbyggede state provider til Blazor UI'et
    builder.Services.AddCascadingAuthenticationState();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "GitHub";
    })
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    })
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["GitHub:ClientId"];
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"];
        options.Scope.Add("user:email");
        // GitHub kræver en callback path, standard er /signin-github
        options.CallbackPath = "/signin-github";
        options.Events.OnCreatingTicket = async context =>
        {
            string? email = context.Identity?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            string? nameIdentifier = context.Identity?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nameIdentifier))
            {
                return;
            }

            IUserService userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

            string UserName = string.Empty;
            string fullName = string.Empty;

            if (string.IsNullOrWhiteSpace(UserName))
            {
                if (string.IsNullOrWhiteSpace(UserName))
                    UserName = context.Identity?.FindFirst(System.Security.Claims.ClaimTypes.GivenName)?.Value ?? string.Empty;

                fullName = context.Identity?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(UserName))
                UserName = fullName;
            
            UserDTO user = await userService.GetOrCreateUser(new UserDTO
            {
                UserName = UserName.Trim(),
                Email = email,
                LoginProvider = "github",
                NameIdentifier = nameIdentifier,
                CreatedAt = DateTime.UtcNow
            }, context.HttpContext.RequestAborted);

            if (!user.Id.HasValue)
            {
                throw new InvalidOperationException(
                    "The user API returned a user without an ID.");
            }

            context.Identity.AddClaim(
                new Claim("app_user_id", user.Id.Value.ToString()));

            context.Identity.AddClaim(
                new Claim(ClaimTypes.Name, user.UserName));

            context.Identity.AddClaim(
                new Claim(ClaimTypes.Email, user.Email));
                };
    });
}

static bool IsSecretSet(IConfiguration configuration)
{
    if (string.IsNullOrEmpty(configuration["GitHub:ClientId"]) || string.IsNullOrEmpty(configuration["GitHub:ClientSecret"]))
    {
        return false;
    }
    return true;
}