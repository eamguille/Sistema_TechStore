using Microsoft.AspNetCore.Mvc;
using Techstore_WebApp.Models.DB;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Techstore_WebApp.Controllers 
{
    public class AccountController : Controller {
        
        private readonly DbTechStoreContext _context;

        public AccountController(DbTechStoreContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string nombreUsuario, string clave)
        {
            if(string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(clave))
            {
                ViewBag.Error = "Usuario y contraseña son requeridos.";
                return View();
            }

            // Aqui buscamos el usuario en la base de datos
            var usuario = _context.Usuarios
                .FirstOrDefault(ele => ele.NombreUsuario == nombreUsuario && ele.Clave == HashPassword(clave));

            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas.";
                return View();
            }

            // Aqui guardamos la informacion del usuario logueado en la sesion creada
            HttpContext.Session.SetString("UsuarioId", usuario.IdUsuario);
            HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("Rol", usuario.Rol);

            // Aqui redireccionamos al usuario segun su rol
            if(usuario.Rol == "administrador" || usuario.Rol == "root")
            {
                Response.Cookies.Append("usuario", "Bienvenido " + usuario.NombreUsuario);
                return RedirectToAction("Index", "Home");
            } else {
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("usuario");
            return RedirectToAction("Login");
        }

        // Creamos un metodo para hashear la clave con sha256
        private string HashPassword(string pwd)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }
    }
}