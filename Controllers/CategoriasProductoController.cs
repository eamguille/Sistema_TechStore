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
    public class CategoriasProductoController : Controller
    {
        private readonly DbTechStoreContext _context;

        public CategoriasProductoController(DbTechStoreContext context)
        {
            _context = context;
        }

        // GET: CategoriasProducto
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoriasProductos.ToListAsync());
        }

        // GET: CategoriasProducto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriasProducto = await _context.CategoriasProductos
                .FirstOrDefaultAsync(m => m.IdCategoriaProducto == id);
            if (categoriasProducto == null)
            {
                return NotFound();
            }

            return View(categoriasProducto);
        }

        // GET: CategoriasProducto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriasProducto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoriaProducto,Categoria")] CategoriasProducto categoriasProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriasProducto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriasProducto);
        }

        // GET: CategoriasProducto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriasProducto = await _context.CategoriasProductos.FindAsync(id);
            if (categoriasProducto == null)
            {
                return NotFound();
            }
            return View(categoriasProducto);
        }

        // POST: CategoriasProducto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoriaProducto,Categoria")] CategoriasProducto categoriasProducto)
        {
            if (id != categoriasProducto.IdCategoriaProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriasProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriasProductoExists(categoriasProducto.IdCategoriaProducto))
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
            return View(categoriasProducto);
        }

        // GET: CategoriasProducto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriasProducto = await _context.CategoriasProductos
                .FirstOrDefaultAsync(m => m.IdCategoriaProducto == id);
            if (categoriasProducto == null)
            {
                return NotFound();
            }

            return View(categoriasProducto);
        }

        // POST: CategoriasProducto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoriasProducto = await _context.CategoriasProductos.FindAsync(id);
            if (categoriasProducto != null)
            {
                _context.CategoriasProductos.Remove(categoriasProducto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriasProductoExists(int id)
        {
            return _context.CategoriasProductos.Any(e => e.IdCategoriaProducto == id);
        }
    }
}
