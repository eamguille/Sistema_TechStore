using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Techstore_WebApp.Models.DB;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión con la base de datos
builder.Services.AddDbContext<DbTechStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));

// Configuración de sesión con tiempo de espera de 5 minutos
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

// Configuración de autenticación por Cookies
builder.Services.AddAuthentication("TechStoreCookie")
    .AddCookie("TechStoreCookie", options => {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(12);
        options.SlidingExpiration = true;
    });

// Configuración de servicios de localización
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configuración de opciones de localización
var supportedCultures = new[] { new CultureInfo("es-ES") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("es-ES");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Agregar servicios para controladores y vistas con localización
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

var app = builder.Build();

// Configuración del pipeline de localización
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

// Habilitar uso de sesiones y autenticación
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Configuración de rutas predeterminadas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
