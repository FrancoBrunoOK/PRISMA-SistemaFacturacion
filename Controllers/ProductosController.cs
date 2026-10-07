
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacion.Models;
using SistemaFacturacion.Data;

public class ProductosController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PRODUCTOS
    // GET: PRODUCTOS
    public async Task<IActionResult> Index(
        string filtroEstado = "Activos",
        string buscar = "",
        int pagina = 1)
    {
        const int tamanioPagina = 10;

        var productos = _context.Productos.AsQueryable();

        // =========================
        // FILTRO POR ESTADO
        // =========================
        if (filtroEstado == "Activos")
        {
            productos = productos.Where(p => p.Activo);
        }
        else if (filtroEstado == "Inactivos")
        {
            productos = productos.Where(p => !p.Activo);
        }

        // =========================
        // BÚSQUEDA
        // =========================
        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();

            productos = productos.Where(p =>
                p.Codigo.Contains(buscar) ||
                p.Descripcion.Contains(buscar));
        }

        // =========================
        // ORDEN
        // =========================
        productos = productos.OrderBy(p => p.Descripcion);

        // =========================
        // PAGINADO
        // =========================
        var totalRegistros = await productos.CountAsync();

        var totalPaginas = (int)Math.Ceiling(
            totalRegistros / (double)tamanioPagina);

        // Evita páginas inválidas
        if (pagina < 1)
        {
            pagina = 1;
        }

        if (totalPaginas > 0 && pagina > totalPaginas)
        {
            pagina = totalPaginas;
        }

        var productosPaginados = await productos
            .Skip((pagina - 1) * tamanioPagina)
            .Take(tamanioPagina)
            .ToListAsync();

        // =========================
        // DATOS PARA LA VISTA
        // =========================
        ViewBag.FiltroEstado = filtroEstado;
        ViewBag.Buscar = buscar;

        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = totalPaginas;
        ViewBag.TotalRegistros = totalRegistros;
        ViewBag.TamanioPagina = tamanioPagina;

        return View(productosPaginados);
    }

    // GET: PRODUCTOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // GET: PRODUCTOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PRODUCTOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Codigo,Descripcion,Precio,IVA,Activo,FechaAlta")] Producto producto)
    {
        if (ModelState.IsValid)
        {
            _context.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(producto);
    }

    // GET: PRODUCTOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }
        return View(producto);
    }

    // POST: PRODUCTOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Codigo,Descripcion,Precio,IVA,Activo,FechaAlta")] Producto producto)
    {
        if (id != producto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.Id))
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
        return View(producto);
    }

    // GET: PRODUCTOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // POST: PRODUCTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto != null)
        {
            producto.Activo = false;

            _context.Update(producto);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProductoExists(int? id)
    {
        return _context.Productos.Any(e => e.Id == id);
    }

    public async Task<IActionResult> Activar(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto != null)
        {
            producto.Activo = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
