using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Techstore_WebApp.Models.DB;

namespace Techstore_WebApp.Controllers
{
    public class VentasController : Controller
    {
        private readonly DbTechStoreContext _context;

        public VentasController(DbTechStoreContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var dbTechStoreContext = _context.VentasEmpresas.Include(v => v.IdClienteNavigation).Include(v => v.IdUsuarioNavigation);
            return View(await dbTechStoreContext.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventasEmpresa = await _context.VentasEmpresas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (ventasEmpresa == null)
            {
                return NotFound();
            }

            return View(ventasEmpresa);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Dui");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,IdCliente,IdUsuario,FechaVenta,TotalVenta,MetodoPago,EstadoVenta")] VentasEmpresa ventasEmpresa)
        {
            var cliente = await _context.Clientes.FindAsync(ventasEmpresa.IdCliente);
            var usuario = await _context.Usuarios.FindAsync(ventasEmpresa.IdUsuario);

            if (cliente != null && usuario != null)
            {
                ventasEmpresa.IdClienteNavigation = cliente;
                ventasEmpresa.IdUsuarioNavigation = usuario;
                _context.Add(ventasEmpresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Dui", ventasEmpresa.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario", ventasEmpresa.IdUsuario);
            return View(ventasEmpresa);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventasEmpresa = await _context.VentasEmpresas.FindAsync(id);
            if (ventasEmpresa == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Dui", ventasEmpresa.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario", ventasEmpresa.IdUsuario);
            return View(ventasEmpresa);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,IdCliente,IdUsuario,FechaVenta,TotalVenta,MetodoPago,EstadoVenta")] VentasEmpresa ventasEmpresa)
        {
            if (id != ventasEmpresa.IdVenta)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(ventasEmpresa.IdCliente);
            var usuario = await _context.Usuarios.FindAsync(ventasEmpresa.IdUsuario);

            if (cliente != null && usuario != null)
            {
                ventasEmpresa.IdClienteNavigation = cliente;
                ventasEmpresa.IdUsuarioNavigation = usuario;
                try
                {
                    _context.Update(ventasEmpresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentasEmpresaExists(ventasEmpresa.IdVenta))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "Dui", ventasEmpresa.IdCliente);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario", ventasEmpresa.IdUsuario);
            return View(ventasEmpresa);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventasEmpresa = await _context.VentasEmpresas
                .Include(v => v.IdClienteNavigation)
                .Include(v => v.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (ventasEmpresa == null)
            {
                return NotFound();
            }

            return View(ventasEmpresa);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ventasEmpresa = await _context.VentasEmpresas.FindAsync(id);
            if (ventasEmpresa != null)
            {
                _context.VentasEmpresas.Remove(ventasEmpresa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentasEmpresaExists(int id)
        {
            return _context.VentasEmpresas.Any(e => e.IdVenta == id);
        }

        [HttpGet]
        public async Task<IActionResult> Buscador(string texto)
        {
            var query = _context.VentasEmpresas
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                query = query.Where(p => p.IdClienteNavigation.NombreCliente.Contains(texto) || p.MetodoPago.Contains(texto));
            }

            var ventas = await query.Select(p => new
            {
                p.IdVenta,
                p.FechaVenta,
                p.TotalVenta,
                p.MetodoPago,
                p.EstadoVenta,
                p.IdClienteNavigation.NombreCliente,
                p.IdUsuarioNavigation.NombreUsuario
            }).ToListAsync();

            return Json(ventas);
        }
    }
}
