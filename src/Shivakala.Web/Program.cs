using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Shivakala.Infrastructure.Data.Seed;
using Shivakala.Infrastructure.Extensions;

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    try
    {
        var raw = databaseUrl.Replace("postgresql://", "postgres://");
        var uri = new Uri(raw);
        var userInfo = uri.UserInfo.Split(':', 2);
        var username = userInfo.Length > 0? userInfo[0] : "";
        var password = userInfo.Length > 1? userInfo[1] : "";
        var database = uri.AbsolutePath.TrimStart('/');
        if (database.Contains("?")) database = database.Split('?')[0];
        var conn = $"Host={uri.Host};Port={uri.Port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true;";
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", conn);
        Environment.SetEnvironmentVariable("Database__Provider", "PostgreSql");
        Console.WriteLine($"[Fix] DB parsed Host={uri.Host} DB={database}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Fix] parse failed: {ex.Message}");
    }
}
var port = Environment.GetEnvironmentVariable("PORT")?? "8080";

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls($"http://+:{port}");

var appDataPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
var dataProtectionPath = Path.Combine(appDataPath, "DataProtection-Keys");
var startupLogPath = Path.Combine(appDataPath, "startup-errors.log");

EnsureDirectory(appDataPath);
EnsureDirectory(dataProtectionPath);

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath)).SetApplicationName("ShivakalaCoaching");
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x => x.MultipartBodyLengthLimit = 20 * 1024 * 1024);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "Shivakala.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.LoginPath = "/admin/login";
    options.AccessDeniedPath = "/access-denied";
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = ctx =>
        {
            var path = ctx.Request.Path.Value?? "";
            if (path.StartsWith("/teacher", StringComparison.OrdinalIgnoreCase))
            {
                var ret = Uri.EscapeDataString(ctx.Request.Path + ctx.Request.QueryString);
                ctx.Response.Redirect($"/teacher/login?returnUrl={ret}");
            }
            else if (path.StartsWith("/parent", StringComparison.OrdinalIgnoreCase))
            {
                var ret = Uri.EscapeDataString(ctx.Request.Path + ctx.Request.QueryString);
                ctx.Response.Redirect($"/parent/login?returnUrl={ret}");
            }
            else { ctx.Response.Redirect(ctx.RedirectUri); }
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = ctx =>
        {
            var user = ctx.HttpContext.User;
            if (user.IsInRole("Teacher")) { ctx.Response.Redirect("/teacher"); return Task.CompletedTask; }
            if (user.IsInRole("Parent")) { ctx.Response.Redirect("/parent"); return Task.CompletedTask; }
            ctx.Response.Redirect("/access-denied");
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddControllersWithViews().AddViewLocalization().AddDataAnnotationsLocalization();
var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("mr") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("mr");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
var app = builder.Build();
var wwwroot = app.Environment.WebRootPath;
var directories = new List<string>
{
    appDataPath, dataProtectionPath,
    Path.Combine(wwwroot, "uploads", "students"),
    Path.Combine(wwwroot, "uploads", "teachers"),
    Path.Combine(wwwroot, "uploads", "homework"),
    Path.Combine(wwwroot, "uploads", "materials"),
    Path.Combine(wwwroot, "uploads", "gallery"),
};
foreach (var dir in directories) EnsureDirectory(dir);
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Home/StatusCodePage", "?code={0}");
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PORT"))) { app.UseHttpsRedirection(); }
app.UseStaticFiles();
app.UseRequestLocalization(app.Services.GetRequiredService<Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions>>().Value);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
try { await DatabaseInitializer.InitializeAsync(app.Services); }
catch (Exception ex) when (!app.Environment.IsDevelopment()) { WriteStartupError(startupLogPath, ex); Console.Error.WriteLine(ex.ToString()); }
await app.RunAsync();
static void EnsureDirectory(string path) { try { Directory.CreateDirectory(path); } catch { } }
static void WriteStartupError(string startupLogPath, Exception exception)
{
    var lines = new[] { $"[{DateTime.UtcNow:O}] Startup initialization failed", exception.ToString(), string.Empty };
    try
    {
        var directory = Path.GetDirectoryName(startupLogPath);
        if (!string.IsNullOrWhiteSpace(directory)) Directory.CreateDirectory(directory);
        File.AppendAllLines(startupLogPath, lines);
    }
    catch { }
    Console.Error.WriteLine(exception);
}
