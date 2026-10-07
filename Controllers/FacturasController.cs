using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacion.Data;
using SistemaFacturacion.Models;
using SistemaFacturacion.Extensions;

namespace SistemaFacturacion.Controllers
{
    public class FacturasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // HISTORIAL DE FACTURAS
        // =====================================================

        public async Task<IActionResult> Index(
            string filtroEstado = "Todas",
            string buscar = "",
            string metodoPago = "Todos",
            DateTime? desde = null,
            DateTime? hasta = null,
            int pagina = 1)
        {
            const int tamanioPagina = 10;

            var facturas = _context.Facturas
                .Include(f => f.Cliente)
                .AsQueryable();

            // =========================
            // FILTRO POR ESTADO
            // =========================
            if (filtroEstado == "Activas")
            {
                facturas = facturas.Where(f => f.Activa);
            }
            else if (filtroEstado == "Anuladas")
            {
                facturas = facturas.Where(f => !f.Activa);
            }

            // =========================
            // BÚSQUEDA
            // =========================
            if (!string.IsNullOrWhiteSpace(buscar))
            {
                buscar = buscar.Trim();

                facturas = facturas.Where(f =>
                    f.Numero.Contains(buscar) ||
                    (f.Cliente != null &&
                     f.Cliente.Nombre.Contains(buscar)));
            }

            // =========================
            // MÉTODO DE PAGO
            // =========================
            if (!string.IsNullOrWhiteSpace(metodoPago) &&
                metodoPago != "Todos")
            {
                facturas = facturas.Where(f =>
                    f.MetodoPago == metodoPago);
            }

            // =========================
            // FECHA DESDE
            // =========================
            if (desde.HasValue)
            {
                facturas = facturas.Where(f =>
                    f.FechaEmision >= desde.Value.Date);
            }

            // =========================
            // FECHA HASTA
            // =========================
            if (hasta.HasValue)
            {
                var fechaLimite = hasta.Value.Date.AddDays(1);

                facturas = facturas.Where(f =>
                    f.FechaEmision < fechaLimite);
            }

            // =========================
            // ORDEN
            // =========================
            facturas = facturas
                .OrderByDescending(f => f.FechaEmision);

            // =========================
            // PAGINADO
            // =========================
            var totalRegistros = await facturas.CountAsync();

            var totalPaginas = (int)Math.Ceiling(
                totalRegistros / (double)tamanioPagina);

            if (pagina < 1)
            {
                pagina = 1;
            }

            if (totalPaginas > 0 && pagina > totalPaginas)
            {
                pagina = totalPaginas;
            }

            var facturasPaginadas = await facturas
                .Skip((pagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ToListAsync();

            // =========================
            // DATOS PARA LA VISTA
            // =========================
            ViewBag.FiltroEstado = filtroEstado;
            ViewBag.Buscar = buscar;
            ViewBag.MetodoPago = metodoPago;
            ViewBag.Desde = desde;
            ViewBag.Hasta = hasta;

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalRegistros = totalRegistros;
            ViewBag.TamanioPagina = tamanioPagina;

            return View(facturasPaginadas);
        }

        // =====================================================
        // PASO 1 - SELECCIONAR CLIENTE
        // =====================================================

        public async Task<IActionResult> Nueva()
        {
            var clientes = await _context.Clientes
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return View(clientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nueva(int clienteId)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c =>
                    c.Id == clienteId &&
                    c.Activo);

            if (cliente == null)
            {
                ModelState.AddModelError(
                    "",
                    "Debe seleccionar un cliente válido.");

                var clientes = await _context.Clientes
                    .Where(c => c.Activo)
                    .OrderBy(c => c.Nombre)
                    .ToListAsync();

                return View(clientes);
            }

            // Creamos una nueva factura temporal
            var facturaActual = new NuevaFacturaViewModel
            {
                ClienteId = cliente.Id,
                ClienteNombre = cliente.Nombre
            };

            // Guardamos la factura temporalmente en Session
            HttpContext.Session.SetObject(
                "FacturaActual",
                facturaActual);

            return RedirectToAction(
                nameof(Productos),
                new { clienteId = cliente.Id });
        }

        // =====================================================
        // PASO 2 - SELECCIONAR PRODUCTOS
        // =====================================================

        public async Task<IActionResult> Productos(int clienteId)
        {
            var facturaActual = HttpContext.Session
                .GetObject<NuevaFacturaViewModel>("FacturaActual");

            if (facturaActual == null ||
                facturaActual.ClienteId != clienteId)
            {
                return RedirectToAction(nameof(Nueva));
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c =>
                    c.Id == clienteId &&
                    c.Activo);

            if (cliente == null)
            {
                return RedirectToAction(nameof(Nueva));
            }

            var productos = await _context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Descripcion)
                .ToListAsync();

            ViewBag.Cliente = cliente;
            ViewBag.FacturaActual = facturaActual;

            return View(productos);
        }

        // =====================================================
        // AGREGAR PRODUCTO
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarProducto(
            int clienteId,
            int productoId,
            decimal cantidad)
        {
            var facturaActual = HttpContext.Session
                .GetObject<NuevaFacturaViewModel>("FacturaActual");

            if (facturaActual == null ||
                facturaActual.ClienteId != clienteId)
            {
                return RedirectToAction(nameof(Nueva));
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p =>
                    p.Id == productoId &&
                    p.Activo);

            if (producto == null)
            {
                return RedirectToAction(
                    nameof(Productos),
                    new { clienteId });
            }

            if (cantidad <= 0)
            {
                cantidad = 1;
            }

            var detalleExistente = facturaActual.Detalles
                .FirstOrDefault(d =>
                    d.ProductoId == producto.Id);

            if (detalleExistente != null)
            {
                // Si ya estaba agregado,
                // sumamos la cantidad
                detalleExistente.Cantidad += cantidad;
            }
            else
            {
                // Si es nuevo, lo agregamos
                var detalle = new DetalleFacturaViewModel
                {
                    ProductoId = producto.Id,
                    Descripcion = producto.Descripcion,
                    PrecioUnitario = producto.Precio,
                    Cantidad = cantidad,
                    IVA = producto.IVA
                };

                facturaActual.Detalles.Add(detalle);
            }

            // Guardamos nuevamente la factura actualizada
            HttpContext.Session.SetObject(
                "FacturaActual",
                facturaActual);

            return RedirectToAction(
                nameof(Productos),
                new { clienteId });
        }

        // =====================================================
        // QUITAR PRODUCTO
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult QuitarProducto(
            int clienteId,
            int productoId)
        {
            var facturaActual = HttpContext.Session
                .GetObject<NuevaFacturaViewModel>("FacturaActual");

            if (facturaActual == null ||
                facturaActual.ClienteId != clienteId)
            {
                return RedirectToAction(nameof(Nueva));
            }

            var detalle = facturaActual.Detalles
                .FirstOrDefault(d =>
                    d.ProductoId == productoId);

            if (detalle != null)
            {
                facturaActual.Detalles.Remove(detalle);

                HttpContext.Session.SetObject(
                    "FacturaActual",
                    facturaActual);
            }

            return RedirectToAction(
                nameof(Productos),
                new { clienteId });
        }

        // =====================================================
        // PASO 3 - RESUMEN
        // =====================================================

        public async Task<IActionResult> Resumen()
        {
            var facturaActual = HttpContext.Session
                .GetObject<NuevaFacturaViewModel>("FacturaActual");

            if (facturaActual == null ||
                !facturaActual.Detalles.Any())
            {
                return RedirectToAction(nameof(Nueva));
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c =>
                    c.Id == facturaActual.ClienteId);

            if (cliente == null)
            {
                return RedirectToAction(nameof(Nueva));
            }

            ViewBag.Cliente = cliente;

            return View(facturaActual);
        }

        // =====================================================
        // CONFIRMAR FACTURA
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmar(
            string? nota,
            string metodoPago)
        {
            var facturaActual = HttpContext.Session
                .GetObject<NuevaFacturaViewModel>("FacturaActual");

            if (facturaActual == null ||
                !facturaActual.Detalles.Any())
            {
                return RedirectToAction(nameof(Nueva));
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c =>
                    c.Id == facturaActual.ClienteId);

            if (cliente == null)
            {
                return RedirectToAction(nameof(Nueva));
            }

            // Guardamos los datos ingresados
            // en el resumen
            facturaActual.Nota = nota;
            facturaActual.MetodoPago = metodoPago;

            // Creamos la factura definitiva
            var factura = new Factura
            {
                ClienteId = cliente.Id,
                FechaEmision = DateTime.Now,
                Numero = string.Empty,
                Subtotal = facturaActual.Subtotal,
                TotalIVA = facturaActual.TotalIVA,
                Total = facturaActual.Total,
                Nota = facturaActual.Nota,
                MetodoPago = facturaActual.MetodoPago
            };

            // Creamos los detalles definitivos
            foreach (var item in facturaActual.Detalles)
            {
                factura.Detalles.Add(
                    new DetalleFactura
                    {
                        ProductoId = item.ProductoId,
                        Descripcion = item.Descripcion,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        IVA = item.IVA,
                        Subtotal = item.Subtotal,
                        ImporteIVA = item.ImporteIVA
                    });
            }

            // Guardamos todo dentro de una transacción
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Facturas.Add(factura);

                await _context.SaveChangesAsync();

                // SQL ya generó el Id.
                // Lo utilizamos para generar
                // el número de factura.
                factura.Numero =
                    factura.Id.ToString("D8");

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            // La factura ya fue confirmada.
            // Eliminamos la factura temporal.
            HttpContext.Session.Remove(
                "FacturaActual");

            return RedirectToAction(
                nameof(Detalle),
                new { id = factura.Id });
        }

        // =====================================================
        // VER FACTURA
        // =====================================================

        public async Task<IActionResult> Detalle(int id)
        {
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // =====================================================
        // DAR DE BAJA / ANULAR FACTURA
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DarDeBaja(int id)
        {
            var factura = await _context.Facturas
                .FirstOrDefaultAsync(f =>
                    f.Id == id);

            if (factura == null)
            {
                return NotFound();
            }

            factura.Activa = false;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}