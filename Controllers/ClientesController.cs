
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacion.Models;
using SistemaFacturacion.Data;

[Authorize]
public class ClientesController : Controller
{
   
    private readonly ApplicationDbContext _context;

    public ClientesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CLIENTES
    public async Task<IActionResult> Index(
        string filtroEstado = "Activos",
        string buscar = "",
        int pagina = 1)
    {
        const int tamanioPagina = 10;

        var clientes = _context.Clientes.AsQueryable();

        // =========================
        // FILTRO POR ESTADO
        // =========================
        if (filtroEstado == "Activos")
        {
            clientes = clientes.Where(c => c.Activo);
        }
        else if (filtroEstado == "Inactivos")
        {
            clientes = clientes.Where(c => !c.Activo);
        }

        // =========================
        // BÚSQUEDA
        // =========================
        if (!string.IsNullOrWhiteSpace(buscar))
        {
            buscar = buscar.Trim();

            clientes = clientes.Where(c =>
                c.Nombre.Contains(buscar) ||
                (c.CUIT != null && c.CUIT.Contains(buscar)) ||
                (c.DNI != null && c.DNI.Contains(buscar)) ||
                (c.Email != null && c.Email.Contains(buscar)) ||
                (c.Telefono != null && c.Telefono.Contains(buscar)));
        }

        // =========================
        // ORDEN
        // =========================
        clientes = clientes.OrderBy(c => c.Nombre);

        // =========================
        // PAGINADO
        // =========================
        var totalRegistros = await clientes.CountAsync();

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

        var clientesPaginados = await clientes
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

        return View(clientesPaginados);
    }

    // GET: CLIENTES/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    // GET: CLIENTES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CLIENTES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,CUIT,DNI,Direccion,Localidad,Provincia,CodigoPostal,Telefono,Email,FechaAlta,Activo")] Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    // GET: CLIENTES/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }
        return View(cliente);
    }

    // POST: CLIENTES/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,CUIT,DNI,Direccion,Localidad,Provincia,CodigoPostal,Telefono,Email,FechaAlta,Activo")] Cliente cliente)
    {
        if (id != cliente.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.Id))
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
        return View(cliente);
    }

    // GET: CLIENTES/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    // POST: CLIENTES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente != null)
        {
            cliente.Activo = false;

            _context.Update(cliente);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ClienteExists(int? id)
    {
        return _context.Clientes.Any(e => e.Id == id);
    }

    public async Task<IActionResult> Activar(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente != null)
        {
            cliente.Activo = true;
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}


