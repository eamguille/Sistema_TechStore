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
    public class ProductosController : Controller
    {
        private readonly DbTechStoreContext _context;

        // Constructor que inicializa el contexto de la base de datos
        public ProductosController(DbTechStoreContext context)
        {
            _context = context;
        }

        // Acción para listar productos
        public async Task<IActionResult> Index()
        {
            var productos = _context.Productos
                .Include(p => p.IdCategoriaProductoNavigation)
                .Include(p => p.IdModeloNavigation)
                .Include(p => p.IdTipoProductoNavigation);

            return View(await productos.ToListAsync());
        }

        // Acción para ver detalles de un producto
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ModelState.AddModelError("", "El ID del producto no puede estar vacío.");
                return NotFound("El ID del producto no puede estar vacío.");
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaProductoNavigation)
                .Include(p => p.IdModeloNavigation)
                .Include(p => p.IdTipoProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (producto == null)
            {
                ModelState.AddModelError("", "El producto solicitado no existe.");
                return NotFound("El producto solicitado no existe.");
            }

            return View(producto);
        }

        // Acción para mostrar el formulario de creación
        public IActionResult Create()
        {
            // Cargar listas desplegables desde la base de datos
            ViewData["IdCategoriaProducto"] = new SelectList(_context.CategoriasProductos, "IdCategoriaProducto", "Categoria");
            ViewData["IdModelo"] = new SelectList(_context.Modelos, "IdModelo", "Modelo1");
            ViewData["IdTipoProducto"] = new SelectList(_context.TiposProductos, "IdTipoProducto", "TipoProducto");
            return View();
        }

        // Acción para manejar la creación de un producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreProducto,DescripcionProducto,IdCategoriaProducto,IdTipoProducto,IdModelo,PrecioCompra,PrecioVenta,CantidadStock,Estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validar relaciones antes de guardar
                    producto.IdCategoriaProductoNavigation = await _context.CategoriasProductos.FindAsync(producto.IdCategoriaProducto);
                    producto.IdModeloNavigation = await _context.Modelos.FindAsync(producto.IdModelo);
                    producto.IdTipoProductoNavigation = await _context.TiposProductos.FindAsync(producto.IdTipoProducto);

                    if (producto.IdCategoriaProductoNavigation == null || producto.IdModeloNavigation == null || producto.IdTipoProductoNavigation == null)
                    {
                        ModelState.AddModelError("", "Relaciones inválidas. Verifique las selecciones.");
                        return View(producto);
                    }

                    // Generar un IDProducto único (asumiendo que se genera aquí)
                    producto.IdProducto = Guid.NewGuid().ToString("N").Substring(0, 8); // Genera un string de 8 caracteres

                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Manejar excepciones y mostrar mensaje de error personalizado
                    ModelState.AddModelError("", $"Error al guardar el producto: {ex.Message}");
                }
            }

            // Si el modelo no es válido, recargar las listas desplegables
            ViewData["IdCategoriaProducto"] = new SelectList(_context.CategoriasProductos, "IdCategoriaProducto", "Categoria", producto.IdCategoriaProducto);
            ViewData["IdModelo"] = new SelectList(_context.Modelos, "IdModelo", "Modelo1", producto.IdModelo);
            ViewData["IdTipoProducto"] = new SelectList(_context.TiposProductos, "IdTipoProducto", "TipoProducto", producto.IdTipoProducto);
            return View(producto);
        }

        // Acción para editar un producto
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ModelState.AddModelError("", "El ID del producto no puede estar vacío.");
                return NotFound("El ID del producto no puede estar vacío.");
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                ModelState.AddModelError("", "El producto solicitado no existe.");
                return NotFound("El producto solicitado no existe.");
            }

            // Cargar listas desplegables con el valor seleccionado
            ViewData["IdCategoriaProducto"] = new SelectList(_context.CategoriasProductos, "IdCategoriaProducto", "Categoria", producto.IdCategoriaProducto);
            ViewData["IdModelo"] = new SelectList(_context.Modelos, "IdModelo", "Modelo1", producto.IdModelo);
            ViewData["IdTipoProducto"] = new SelectList(_context.TiposProductos, "IdTipoProducto", "TipoProducto", producto.IdTipoProducto);
            return View(producto);
        }

        // Acción para manejar la edición de un producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdProducto,NombreProducto,DescripcionProducto,IdCategoriaProducto,IdTipoProducto,IdModelo,PrecioCompra,PrecioVenta,CantidadStock,Estado")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                ModelState.AddModelError("", "El ID del producto no coincide con el producto enviado.");
                return NotFound("El ID del producto no coincide con el producto enviado.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        ModelState.AddModelError("", "El producto ya no existe.");
                        return NotFound("El producto ya no existe.");
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    // Manejar excepciones y mostrar mensaje de error personalizado
                    ModelState.AddModelError("", $"Error al editar el producto: {ex.Message}");
                }
            }

            // Si el modelo no es válido, recargar las listas desplegables
            ViewData["IdCategoriaProducto"] = new SelectList(_context.CategoriasProductos, "IdCategoriaProducto", "Categoria", producto.IdCategoriaProducto);
            ViewData["IdModelo"] = new SelectList(_context.Modelos, "IdModelo", "Modelo1", producto.IdModelo);
            ViewData["IdTipoProducto"] = new SelectList(_context.TiposProductos, "IdTipoProducto", "TipoProducto", producto.IdTipoProducto);
            return View(producto);
        }

        // Acción para eliminar un producto
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ModelState.AddModelError("", "El ID del producto no puede estar vacío.");
                return NotFound("El ID del producto no puede estar vacío.");
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaProductoNavigation)
                .Include(p => p.IdModeloNavigation)
                .Include(p => p.IdTipoProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);

            if (producto == null)
            {
                ModelState.AddModelError("", "El producto solicitado no existe.");
                return NotFound("El producto solicitado no existe.");
            }

            return View(producto);
        }

        // Acción para confirmar la eliminación de un producto
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                try
                {
                    _context.Productos.Remove(producto);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Manejar excepciones y mostrar mensaje de error personalizado
                    ModelState.AddModelError("", $"Error al eliminar el producto: {ex.Message}");
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }
            else
            {
                ModelState.AddModelError("", "El producto ya no existe.");
            }

            return RedirectToAction(nameof(Index));
        }

        // Método para verificar si un producto existe
        private bool ProductoExists(string id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }
    }
}
