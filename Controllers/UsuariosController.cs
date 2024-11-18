using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Techstore_WebApp.Models.DB;
using System.Security.Cryptography;
using System.Text;

namespace Techstore_WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly DbTechStoreContext _context;

        public UsuariosController(DbTechStoreContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,Nombres,Apellidos,NombreUsuario,Clave,Email,Telefono,Direccion,Rol")] Usuario usuario)
        {
            if (usuario.IdUsuario == null)
            {
                usuario.IdUsuario = GenerarID();
                usuario.Clave = EncriptarClaveGenerada(usuario.Clave);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdUsuario,Nombres,Apellidos,NombreUsuario,Clave,Email,Telefono,Direccion,Rol")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioExistente = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);

                    if (usuarioExistente == null)
                    {
                        return NotFound();
                    }

                    if (string.IsNullOrWhiteSpace(usuario.Clave))
                    {
                        usuario.Clave = usuarioExistente.Clave;
                    }
                    else
                    {
                        usuario.Clave = EncriptarClaveGenerada(usuario.Clave);
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(string id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

        private string GenerarID()
        {
            var ultimoUsuario = _context.Usuarios
            .OrderByDescending(u => u.IdUsuario)
            .FirstOrDefault();

            if (ultimoUsuario == null)
            {
                return "U00001";
            }

            string ultimoID = ultimoUsuario.IdUsuario;
            int numero;

            if (int.TryParse(ultimoID.Substring(1), out numero))
            {
                numero++;
                return $"U{numero:D5}";
            }

            return "U00001";
        }

        private string EncriptarClaveGenerada(string clave)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(clave);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder resultado = new StringBuilder();
                foreach (byte b in hash)
                {
                    resultado.Append(b.ToString("x2"));
                }
                return resultado.ToString();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Buscador(string texto)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                query = query.Where(c =>
                    c.Nombres.Contains(texto) ||
                    c.Apellidos.Contains(texto) ||
                    c.NombreUsuario.Contains(texto) ||
                    c.Email.Contains(texto) ||
                    c.Telefono.Contains(texto) ||
                    c.Direccion.Contains(texto) ||
                    c.Rol.Contains(texto));
            }

            var usuarios = await query.Select(c => new
            {
                c.IdUsuario,
                c.Nombres,
                c.Apellidos,
                c.NombreUsuario,
                c.Email,
                c.Telefono,
                c.Direccion,
                c.Rol
            }).ToListAsync();

            return Json(usuarios);
        }
    }
}
