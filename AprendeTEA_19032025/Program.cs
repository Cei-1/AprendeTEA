using AprendeTEA_19032025.BL;
using AprendeTEA_19032025.Data;
using AprendeTEA_19032025.Helpers;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Configure request body size limits for file uploads
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10485760; // 10 MB
    options.ValueLengthLimit = 10485760; // 10 MB
});



// MVC
builder.Services.AddControllersWithViews();

// ?? Autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";          // a dónde mandar si no está logueado
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/AccessDenied";

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // en producción con HTTPS
        options.Cookie.SameSite = SameSiteMode.Lax;              // o Strict si solo usas mismo dominio
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// (Opcional) Session si quieres guardar otros datos
builder.Services.AddSession();

builder.Services.AddScoped<PlanTrabajo>();
builder.Services.AddScoped<Unidad>();


//filtro global para que todo requiera login por default,
//Con eso ya no hace falta poner [Authorize] en cada controller, solo [AllowAnonymous] donde quieras acceso público.
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});


//Hangfire
// 1) Enlazar EmailSettings desde appsettings.json
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// 2) Registrar el servicio de correo
builder.Services.AddTransient<AprendeTEA_19032025.Helpers.IEmailSender, EmailSender>();
builder.Services.AddTransient<AprendeTEA_19032025.Helpers.EmailJobs>();


// 3) Configurar Hangfire
builder.Services.AddHangfire(config =>
{
    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(
              builder.Configuration.GetConnectionString("ConexionSQL"));
});

builder.Services.AddHangfireServer();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ?? Muy importante: primero UseAuthentication, luego UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() } // si ya la creaste
});

// Job recurrente opcional para limpiar tokens expirados
Hangfire.RecurringJob.AddOrUpdate(
    "limpiar-tokens-expirados",
    () => AprendeTEA_19032025.BL.Usuario.LimpiarTokensExpirados(),
    Cron.Daily);


app.Run();
