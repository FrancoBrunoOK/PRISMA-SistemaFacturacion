
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacion.Data;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// BASE DE DATOS
// ==========================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// ASP.NET CORE IDENTITY
// ==========================================

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Configuración de contraseñas
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    // Los nombres de usuario deben ser únicos
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configuración de las cookies de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuenta/Login";
    options.AccessDeniedPath = "/Cuenta/AccesoDenegado";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// ==========================================
// MVC Y SESIONES
// ==========================================

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

// ==========================================
// DATOS DE PRUEBA
// ==========================================


// ==========================================
// INICIALIZACIÓN DE DATOS Y ROLES
// ==========================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services
            .GetRequiredService<ApplicationDbContext>();

        // Datos de prueba de facturación
        DbInitializer.Initialize(context);

        // Roles de seguridad
        await IdentityInitializer.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services
            .GetRequiredService<ILogger<Program>>();

        logger.LogError(
            ex,
            "Error al inicializar datos o roles.");
        throw;
    }
}

// ==========================================
// PIPELINE HTTP
// ==========================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Sesiones del proceso de facturación
app.UseSession();

// Autenticación: identifica al usuario
app.UseAuthentication();

// Autorización: verifica sus permisos
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
