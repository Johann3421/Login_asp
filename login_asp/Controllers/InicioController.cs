using Microsoft.AspNetCore.Mvc;

using login_asp.Models;
using login_asp.Recursos;
using login_asp.Servicios.Contrato;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace login_asp.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;

        public InicioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);

            Usuario usuario_creado = await _usuarioServicio.SaveUsuarios(modelo);

            if(usuario_creado.IdUsuario> 0)
                return RedirectToAction("IniciarSesion","Inicio");

            ViewData["Mensaje"] = "No se pudo crear el Usuario";
            return View();
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {

            Usuario usuario_encontrado = await _usuarioServicio.GetUsuarios(correo,Utilidades.EncriptarClave(clave));
            if(usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se Encontraron Coincidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Index","Home");
        }
        public IActionResult EditarUsuario(int id)
            
            {
            
                return View();
            }

        [HttpPost]
        public async Task<IActionResult> EditarUsuario(Usuario modelo)
        {
            // Asegúrate de que el ID del usuario esté presente
            if (modelo.IdUsuario == 0)
            {
                // Manejo del caso en el que el ID del usuario no esté presente
                return NotFound(); // Otra respuesta si el usuario no se encuentra
            }

            try
            {
                // Encripta la nueva contraseña si es necesario
                if (!string.IsNullOrEmpty(modelo.Clave))
                {
                    modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);
                }

                // Actualiza el usuario en la base de datos
                bool usuarioActualizado = await _usuarioServicio.ActualizarUsuario(modelo);

                if (usuarioActualizado)
                    return RedirectToAction("IniciarSesion", "Inicio");

                ViewData["Mensaje"] = "No se pudo actualizar el Usuario";
            }
            catch (Exception)
            {
                // Manejo de errores, si es necesario
                ViewData["Mensaje"] = "Ocurrió un error al intentar actualizar el Usuario";
            }

            // Si llegamos aquí, significa que hubo un error al actualizar el usuario
            return View(modelo); // Puedes mostrar un mensaje de error en la vista
        }

    }
}
