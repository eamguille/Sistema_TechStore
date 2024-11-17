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

    // Obtener el total de ventas agrupadas por mes y año
    [HttpGet]
    public async Task<IActionResult> ObtenerVentasPorMes()
    {
        try
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

            return Json(datos);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en ObtenerVentasPorMes: {ex.Message}");
            return BadRequest(new { Message = "Error al obtener las ventas por mes.", Detalle = ex.Message });
        }
    }

    // Obtener los 5 productos más vendidos
    [HttpGet]
    public async Task<IActionResult> Obtener5ProductosMasVendidos()
    {
        try
        {
            var productosMasVendidos = await _context.DetallesVentas
                .Join(
                    _context.Productos,
                    detalle => detalle.IdProducto,     // Clave externa en DetallesVentas
                    producto => producto.IdProducto,   // Clave primaria en Productos
                    (detalle, producto) => new         // Resultado de la unión
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error en Obtener5ProductosMasVendidos: {ex.Message}");
            return BadRequest(new { Message = "Error al obtener los productos más vendidos.", Detalle = ex.Message });
        }
    }
}
