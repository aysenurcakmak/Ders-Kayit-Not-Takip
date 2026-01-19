using Microsoft.EntityFrameworkCore;
using DersKayitNotTakip.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// --- EKLENEN KRÝTÝK KISIM ---
// Layout sayfasýnda (View tarafýnda) Session okumak için bu servis ÞART:
builder.Services.AddHttpContextAccessor();
// ----------------------------

builder.Services.AddSession(); // Sende zaten vardý, aynen kalýyor.

builder.Services.AddDbContext<OdevContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware'i Routing ile Authorization arasýnda olmalý (Sende doðru yerde)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();