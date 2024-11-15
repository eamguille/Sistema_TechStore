using Microsoft.AspNetCore.Mvc;
using Techstore_WebApp.Models.DB;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Login(string nombreUsuario, string clave)
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

            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("IdUsuario", usuario.IdUsuario.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, "TechStoreCookie");
            var authProperties = new AuthenticationProperties {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(12)
            };

            await HttpContext.SignInAsync("TechStoreCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

            // Aqui redireccionamos al usuario segun su rol
            if(usuario.Rol == "administrador" || usuario.Rol == "root" || usuario.Rol == "empleado")
            {
                Response.Cookies.Append("usuario", "Hola de nuevo! " + usuario.Nombres);
                return RedirectToAction("Index", "Home");
            } else {
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("TechStoreCookie");
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

        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("IdUsuario,Nombres,Apellidos,NombreUsuario,Clave,Email,Telefono,Direccion,Rol")] Usuario usuario)
        {
            if (usuario.IdUsuario == null)
            {
                usuario.IdUsuario = GenerarID();
                usuario.Clave = EncriptarClaveGenerada(usuario.Clave);
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(usuario);
        }

        [Authorize]
        public IActionResult Profile() {
            var nombreUsuario = User.FindFirstValue(ClaimTypes.Name);

            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);

            if(usuario == null) {
                return NotFound("Usuario no encontrado.");
            }

            return View(usuario);
        }

        private string GenerarID() {
            var ultimoUsuario = _context.Usuarios
            .OrderByDescending(u => u.IdUsuario)
            .FirstOrDefault();

            if(ultimoUsuario == null){
                return "U00001";
            }

            string ultimoID = ultimoUsuario.IdUsuario;
            int numero;

            if(int.TryParse(ultimoID.Substring(1), out numero)){
                numero++;
                return $"U{numero:D5}";
            }

            return "U00001";
        }

        private string EncriptarClaveGenerada(string clave) {
            using(SHA256 sha256 = SHA256.Create()) {
                byte[] bytes = Encoding.UTF8.GetBytes(clave);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder resultado = new StringBuilder();
                foreach (byte b in hash) {
                    resultado.Append(b.ToString("x2"));
                }
                return resultado.ToString();
            }
        }
    }
}