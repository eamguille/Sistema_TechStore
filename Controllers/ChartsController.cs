using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Techstore_WebApp.Models.DB;

public class ChartsController : Controller
{
    private readonly DbTechStoreContext _context;

    public ChartsController(DbTechStoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerVentasPorMes()
    {
        var datos = await _context.VentasEmpresas
                .GroupBy(v => new { v.FechaVenta.Year, v.FechaVenta.Month })
                .Select(g => new
                {
                    Anio = g.Key.Year,
                    Mes = g.Key.Month,
                    TotalVentas = g.Sum(v => v.TotalVenta)
                })
                .OrderBy(d => d.Anio).ThenBy(d => d.Mes)
                .ToListAsync();

        Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(datos));
        return Json(datos);
    }
}