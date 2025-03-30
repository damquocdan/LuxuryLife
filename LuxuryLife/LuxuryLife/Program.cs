
using LuxuryLife.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("LuxuryLifeConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<TourBookingContext>(options =>
    options.UseSqlServer(connectionString));

// Đọc cấu hình PayOS từ appsettings.json
var payOsConfig = builder.Configuration.GetSection("PayOS");
var clientId = payOsConfig["ClientId"];
var apiKey = payOsConfig["ApiKey"];
var checksumKey = payOsConfig["ChecksumKey"];

// Khởi tạo PayOS
var payOS = new PayOS(clientId, apiKey, checksumKey);
builder.Services.AddSingleton(payOS);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ChatbotService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "LuxuryLife";
});
builder.Services.AddDistributedMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
