
using SistemaFacturacion.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SistemaFacturacion.Controllers
{
    [Authorize(Roles = "Superusuario")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ==========================================
        // LISTADO DE USUARIOS
        // ==========================================

        public async Task<IActionResult> Index()
        {
            var usuarios = _userManager.Users
                .OrderBy(u => u.Email)
                .ToList();

            var listado = new List<UsuarioListadoViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager
                    .GetRolesAsync(usuario);

                listado.Add(new UsuarioListadoViewModel
                {
                    Id = usuario.Id,
                    Email = usuario.Email ?? "",
                    Roles = string.Join(", ", roles),
                    Bloqueado = await _userManager
                        .IsLockedOutAsync(usuario)
                });
            }

            return View(listado);
        }

        // ==========================================
        // CREAR USUARIO - GET
        // ==========================================

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new CrearUsuarioViewModel());
        }

        // ==========================================
        // CREAR USUARIO - POST
        // ==========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            CrearUsuarioViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            // Validamos el rol recibido
            if (modelo.Rol != "Usuario" &&
                modelo.Rol != "Superusuario")
            {
                ModelState.AddModelError(
                    nameof(modelo.Rol),
                    "El rol seleccionado no es válido.");

                return View(modelo);
            }

            // Verificamos que el rol exista
            if (!await _roleManager.RoleExistsAsync(modelo.Rol))
            {
                ModelState.AddModelError(
                    nameof(modelo.Rol),
                    "El rol seleccionado no existe.");

                return View(modelo);
            }

            // Evitamos correos duplicados
            var usuarioExistente = await _userManager
                .FindByEmailAsync(modelo.Email.Trim());

            if (usuarioExistente != null)
            {
                ModelState.AddModelError(
                    nameof(modelo.Email),
                    "Ya existe un usuario con ese correo electrónico.");

                return View(modelo);
            }

            // Creamos la cuenta con Identity
            var usuario = new IdentityUser
            {
                UserName = modelo.Email.Trim(),
                Email = modelo.Email.Trim()
            };

            var resultado = await _userManager
                .CreateAsync(usuario, modelo.Password);

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(modelo);
            }

            // Asignamos el rol seleccionado
            var resultadoRol = await _userManager
                .AddToRoleAsync(usuario, modelo.Rol);

            if (!resultadoRol.Succeeded)
            {
                // Evitamos dejar una cuenta sin rol
                var eliminacion = await _userManager
                    .DeleteAsync(usuario);

                foreach (var error in resultadoRol.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                if (!eliminacion.Succeeded)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "No se pudo revertir la creación. Revisá la cuenta desde la administración.");
                }

                return View(modelo);
            }

            TempData["MensajeExito"] =
                "El usuario fue creado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}
