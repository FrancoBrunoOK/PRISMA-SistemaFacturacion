
using Microsoft.AspNetCore.Identity;

namespace SistemaFacturacion.Data
{
    public static class IdentityInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            // Crear roles si no existen
            string[] roles = { "Superusuario", "Usuario" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"Error al crear el rol {role}: " +
                            string.Join(", ",
                                result.Errors.Select(e => e.Description)));
                    }
                }
            }

            // Credenciales iniciales desde configuración
            var configuration = serviceProvider
                .GetRequiredService<IConfiguration>();

            var email = configuration["AdminSeed:Email"];
            var password = configuration["AdminSeed:Password"];

            // Si no se configuró el administrador, no crearlo
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(
                    admin, password);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Error al crear el Superusuario: " +
                        string.Join(", ",
                            result.Errors.Select(e => e.Description)));
                }
            }

            if (!await userManager.IsInRoleAsync(
                admin, "Superusuario"))
            {
                var result = await userManager.AddToRoleAsync(
                    admin, "Superusuario");

                if (!result.Succeeded)
                {
                    throw new Exception(
                        "Error al asignar el rol Superusuario.");
                }
            }
        }
    }
}
