using FancySignup.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. ADD SERVICES TO THE CONTAINER
// ==========================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

// Register SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 2. BUILD THE APP (This line was missing/too late)
// ==========================================
var app = builder.Build();

// ==========================================
// 3. CONFIGURE THE HTTP REQUEST PIPELINE
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Static files must be loaded before routing
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); // (Optional: Good practice to keep if you add login later)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// This is required for [Route("api/[controller]")] to work!
app.MapControllers(); 

app.Run();