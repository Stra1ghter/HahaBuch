using HahaBuch;
using HahaBuch.Category;
using HahaBuch.Client;
using HahaBuch.Components;
using HahaBuch.Components.Account;
using HahaBuch.Data;
using HahaBuch.SharedContracts;
using HahaBuch.Transaction;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<ApplicationDbContext>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<VaultAccessor, VaultAccessor>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddControllers();
builder.Services.AddLocalization();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies(options =>
    {
        options?.ApplicationCookie?.Configure(cookieOptions =>
        {
            cookieOptions.Cookie.HttpOnly = true;
            cookieOptions.ExpireTimeSpan = TimeSpan.FromDays(30);
        });
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperUser", policy => policy.RequireClaim("IsSuperUser", "true"));
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUserEntity>(options =>
    {
        // No real requirements during testing phase
        options.SignIn.RequireConfirmedAccount = true;
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedEmail = false;

        // Password settings:
        // Don't use very strict settings that will enforce bad user beheavior like writing down passwords or always putting ! at the end of a password.
        // Simply require a long password and a mix of lower- and uppercase. (But allow other characters as well.)
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 9;
        options.Password.RequiredUniqueChars = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
    
        options.Lockout.MaxFailedAccessAttempts = 50;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUserEntity>, IdentityNoOpEmailSender>();

// Configure forwarded headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    //options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseHttpsRedirection();
}
else if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // If you want to run HahaBuch with kestrel being internet-facing, then enable the following code.
    // app.UseHsts();
    // app.UseHttpsRedirection();

    // Else we assume that a reverse proxy (e.g. nginx) is used that handles TLS, HSTS, and HTTPS redirection.
    // Read forwarded headers to correctly determine the originating IP and protocol.
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        KnownNetworks = { new Microsoft.AspNetCore.HttpOverrides.IPNetwork(IPAddress.Parse("172.20.0.0"), 16) },
    });
}

app.UseRequestLocalization(new RequestLocalizationOptions()
    .AddSupportedCultures("en-US", "de-DE")
    .AddSupportedUICultures("en-US", "de-DE"));

app.UseAntiforgery();
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HahaBuch.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await app.SetupDatabaseAsync();

app.Run();