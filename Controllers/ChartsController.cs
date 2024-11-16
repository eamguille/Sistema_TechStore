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

    [HttpGet]
    public async Task<IActionResult> Obtener5ProductosMasVendidos()
    {
        var productosMasVendidos = await _context.DetallesVentas
        .Join(_context.Productos,
              detalle => detalle.IdProducto,
              producto => producto.IdProducto,
              (detalle, producto) => new
              {
                  producto.NombreProducto,
                  detalle.Cantidad,
                  detalle.PrecioUnitario
              })
        .GroupBy(x => x.NombreProducto)
        .Select(g => new
        {
            NombreProducto = g.Key,
            CantidadTotal = g.Sum(x => x.Cantidad),
            PrecioPromedio = g.Average(x => x.PrecioUnitario)
        })
        .OrderByDescending(x => x.CantidadTotal)
        .Take(5)
        .ToListAsync();

        return Json(productosMasVendidos);
    }
}