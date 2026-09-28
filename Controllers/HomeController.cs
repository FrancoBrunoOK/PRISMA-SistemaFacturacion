using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacion.Data;
using SistemaFacturacion.Models;

namespace SistemaFacturacion.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                ClientesActivos = await _context.Clientes
                    .CountAsync(c => c.Activo),

                ProductosActivos = await _context.Productos
                    .CountAsync(p => p.Activo),

                FacturasActivas = await _context.Facturas
                    .CountAsync(f => f.Activa),

                FacturasAnuladas = await _context.Facturas
                    .CountAsync(f => !f.Activa),

                TotalFacturado = await _context.Facturas
                    .Where(f => f.Activa)
                    .SumAsync(f => (decimal?)f.Total) ?? 0
            };

            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}