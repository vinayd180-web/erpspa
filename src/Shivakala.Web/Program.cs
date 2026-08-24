using Microsoft.EntityFrameworkCore;
using Shivakala.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Render PORT fix
var port = Environment.GetEnvironmentVariable("PORT")?? "10000";
builder.WebHost.UseUrls($"http://+:{port}");

// DB Connection - Support DATABASE_URL and DB_HOST both
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");

if (!string.IsNullOrEmpty(databaseUrl))
{
    try
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
        Console.WriteLine($"[Fix] DB from DATABASE_URL Host={uri.Host}");
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
    Console.WriteLine($"[Fix] DB from DB_HOST Host={dbHost} DB={dbName}");
}

builder.Services.AddDbContext<ShivakalaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --- YEHI FIX HAI - Table auto banega ---
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ShivakalaDbContext>();
        db.Database.EnsureCreated();
        Console.WriteLine("[Fix] Database EnsureCreated executed - Courses table created");
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
