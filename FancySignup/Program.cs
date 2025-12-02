using FancySignup.Data;
using FancySignup.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
// ==========================================
// 1. ADD SERVICES TO THE CONTAINER
// ==========================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

// SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient for APIs
builder.Services.AddHttpClient();

// SESSION
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// DB + admin seeding
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();

    var admin = db.People.FirstOrDefault(p => p.Email == "admin@pokedex.com");

    if (admin != null)
    {
        admin.IsAdmin = true;
    }
    else
    {
        db.People.Add(new Person
        {
            Email = "admin@pokedex.com",
            FirstName = "admin",
            LastName = "admin",
            CountryId = 1,
            Password = "Admin123!",
            IsAdmin = true
        });
    }

    db.SaveChanges();
}

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
app.UseSession();

app.UseAuthorization(); // (Optional: Good practice to keep if you add login later)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// This is required for [Route("api/[controller]")] to work!
app.MapControllers(); 

app.Run();