using Kitchen.Data;
using Microsoft.EntityFrameworkCore;
using static Kitchen.Controllers.RecipeController;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Add MVC
builder.Services.AddControllersWithViews();

// ✅ Add Session Services
builder.Services.AddDistributedMemoryCache(); // Required for session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Optional: session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

var app = builder.Build();

// ✅ Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<LanguageMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ✅ Enable Session BEFORE Authorization
app.UseSession();

app.UseAuthorization();

// ✅ Setup routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Recipe}/{action=Index}/{id?}");

app.Run();
