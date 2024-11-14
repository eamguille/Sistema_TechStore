using Microsoft.EntityFrameworkCore;
using Techstore_WebApp.Models.DB;

var builder = WebApplication.CreateBuilder(args);

// Aqui agregamos la cadena de conexion creada en appsettings.json al archivo program.cs
builder.Services.AddDbContext<DbTechStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
