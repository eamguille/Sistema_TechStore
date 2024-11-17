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
    public class ComprasController : Controller
    {
        private readonly DbTechStoreContext _context;

        public ComprasController(DbTechStoreContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            var dbTechStoreContext = _context.ComprasEmpresas.Include(c => c.IdProveedorNavigation).Include(c => c.IdUsuarioNavigation);
            return View(await dbTechStoreContext.ToListAsync());
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprasEmpresa = await _context.ComprasEmpresas
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (comprasEmpresa == null)
            {
                return NotFound();
            }

            return View(comprasEmpresa);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Dui");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario");
            return View();
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,IdProveedor,IdUsuario,FechaCompra,TotalCompra,EstadoCompra")] ComprasEmpresa comprasEmpresa)
        {
            var proveedor = await _context.Proveedores.FindAsync(comprasEmpresa.IdProveedor);
            var usuario = await _context.Usuarios.FindAsync(comprasEmpresa.IdUsuario);
            if (proveedor != null && usuario != null)
            {
                comprasEmpresa.IdProveedorNavigation = proveedor;
                comprasEmpresa.IdUsuarioNavigation = usuario;
                _context.Add(comprasEmpresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Dui", comprasEmpresa.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "ombreUsuario", comprasEmpresa.IdUsuario);
            return View(comprasEmpresa);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprasEmpresa = await _context.ComprasEmpresas.FindAsync(id);
            if (comprasEmpresa == null)
            {
                return NotFound();
            }
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Dui", comprasEmpresa.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario", comprasEmpresa.IdUsuario);
            return View(comprasEmpresa);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,IdProveedor,IdUsuario,FechaCompra,TotalCompra,EstadoCompra")] ComprasEmpresa comprasEmpresa)
        {
            if (id != comprasEmpresa.IdCompra)
            {
                return NotFound();
            }
            var proveedor = await _context.Proveedores.FindAsync(comprasEmpresa.IdProveedor);
            var usuario = await _context.Usuarios.FindAsync(comprasEmpresa.IdUsuario);

            if (proveedor != null && usuario != null)
            {
                comprasEmpresa.IdProveedorNavigation = proveedor;
                comprasEmpresa.IdUsuarioNavigation = usuario;
                try
                {
                    _context.Update(comprasEmpresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprasEmpresaExists(comprasEmpresa.IdCompra))
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
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "Dui", comprasEmpresa.IdProveedor);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario", comprasEmpresa.IdUsuario);
            return View(comprasEmpresa);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comprasEmpresa = await _context.ComprasEmpresas
                .Include(c => c.IdProveedorNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (comprasEmpresa == null)
            {
                return NotFound();
            }

            return View(comprasEmpresa);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comprasEmpresa = await _context.ComprasEmpresas.FindAsync(id);
            if (comprasEmpresa != null)
            {
                _context.ComprasEmpresas.Remove(comprasEmpresa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComprasEmpresaExists(int id)
        {
            return _context.ComprasEmpresas.Any(e => e.IdCompra == id);
        }

        [HttpGet]
        public async Task<IActionResult> Buscador(string texto)
        {
            var query = _context.ComprasEmpresas
                .Include(p => p.IdProveedorNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                query = query.Where(p => p.IdProveedorNavigation.NombreProveedor.Contains(texto) || p.EstadoCompra.Contains(texto));
            }

            var compras = await query.Select(p => new
            {
                p.IdCompra,
                p.FechaCompra,
                p.TotalCompra,
                p.EstadoCompra,
                p.IdProveedorNavigation.NombreProveedor,
                p.IdUsuarioNavigation.NombreUsuario
            }).ToListAsync();

            return Json(compras);
        }
    }
}
