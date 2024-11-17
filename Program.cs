using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Techstore_WebApp.Models.DB;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la cadena de conexi�n con la base de datos
builder.Services.AddDbContext<DbTechStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// Configuraci�n de sesi�n con tiempo de espera de 5 minutos
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

// Configuraci�n de autenticaci�n por Cookies
builder.Services.AddAuthentication("TechStoreCookie")
    .AddCookie("TechStoreCookie", options => {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(12);
        options.SlidingExpiration = true;
    });

// Configuraci�n de servicios de localizaci�n
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configuraci�n de opciones de localizaci�n
var supportedCultures = new[] { new CultureInfo("es-ES") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-ES");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Agregar servicios para controladores y vistas con localizaci�n
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

var app = builder.Build();

// Configuraci�n del pipeline de localizaci�n
var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar uso de sesiones y autenticaci�n
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Configuraci�n de rutas predeterminadas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
