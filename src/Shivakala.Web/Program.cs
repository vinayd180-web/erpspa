using Microsoft.EntityFrameworkCore;
using Shivakala.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Fix for Render PORT
var port = Environment.GetEnvironmentVariable("PORT")?? "10000";
builder.WebHost.UseUrls($"http://+:{port}");

// DB Connection Fix - Support both DATABASE_URL and DB_HOST
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");

if (!string.IsNullOrEmpty(databaseUrl))
{
    // Render gives postgres://user:pass@host:port/db
    try
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
        Console.WriteLine($"[Fix] DB from DATABASE_URL Host={uri.Host} DB={uri.AbsolutePath.TrimStart('/')}");
    }
    catch { }
}
else if (!string.IsNullOrEmpty(dbHost))
{
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT")?? "5432";
    var dbName = Environment.GetEnvironmentVariable("DB_DATABASE")?? Environment.GetEnvironmentVariable("DB_NAME")?? "spa_4ic5";
    var dbUser = Environment.GetEnvironmentVariable("DB_USERNAME")?? Environment.GetEnvironmentVariable("DB_USER")?? "spa";
    var dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD")?? Environment.GetEnvironmentVariable("DB_PASS")?? "";

    connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPass};SSL Mode=Require;Trust Server Certificate=true;";
    Console.WriteLine($"[Fix] DB from DB_HOST vars Host={dbHost} DB={dbName} Port={dbPort} User={dbUser}");
}

// Add services
builder.Services.AddDbContext<ShivakalaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllersWithViews();

// Add Infrastructure Services - adjust if your service names are different
builder.Services.AddScoped<Shivakala.Infrastructure.Repositories.ICourseRepository, Shivakala.Infrastructure.Repositories.CourseRepository>();
builder.Services.AddScoped<Shivakala.Infrastructure.Services.ICourseService, Shivakala.Infrastructure.Services.CourseService>();
builder.Services.AddScoped<Shivakala.Infrastructure.Services.IHomePageService, Shivakala.Infrastructure.Services.HomePageService>();

var app = builder.Build();

// --- AUTO CREATE TABLES FIX - Isse Courses does not exist error khatam hoga ---
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ShivakalaDbContext>();
        db.Database.EnsureCreated();
        Console.WriteLine("[Fix] Database EnsureCreated executed");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[Fix] EnsureCreated failed: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
