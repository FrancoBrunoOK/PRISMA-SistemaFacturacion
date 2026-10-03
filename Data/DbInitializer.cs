using SistemaFacturacion.Models;

namespace SistemaFacturacion.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Si ya hay datos, no vuelve a cargar
            if (context.Clientes.Any() || context.Productos.Any())
            {
                return;
            }

            // =====================
            // CLIENTES
            // =====================
            var clientes = new Cliente[]
            {
                new Cliente
                {
                    Nombre = "García y Asociados S.R.L.",
                    CUIT = "30-71234567-8",
                    Direccion = "Av. Corrientes 1234",
                    Localidad = "CABA",
                    Provincia = "CABA",
                    CodigoPostal = "1043",
                    Telefono = "11-4567-8901",
                    Email = "contacto@garciaasoc.com.ar",
                    FechaAlta = new DateTime(2025, 3, 10),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "María Elena Rodríguez",
                    DNI = "28456789",
                    Direccion = "Calle San Martín 456",
                    Localidad = "Rosario",
                    Provincia = "Santa Fe",
                    CodigoPostal = "2000",
                    Telefono = "341-555-1234",
                    Email = "maria.rodriguez@email.com",
                    FechaAlta = new DateTime(2025, 4, 15),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "Tecnología del Sur S.A.",
                    CUIT = "30-69874521-3",
                    Direccion = "Ruta 9 Km 45",
                    Localidad = "Córdoba",
                    Provincia = "Córdoba",
                    CodigoPostal = "5000",
                    Telefono = "351-489-7654",
                    Email = "ventas@tecsur.com.ar",
                    FechaAlta = new DateTime(2025, 5, 2),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "Juan Carlos Pérez",
                    DNI = "32145678",
                    Direccion = "Belgrano 789",
                    Localidad = "Mendoza",
                    Provincia = "Mendoza",
                    CodigoPostal = "5500",
                    Telefono = "261-423-9876",
                    Email = "juancarlos.perez@gmail.com",
                    FechaAlta = new DateTime(2025, 6, 20),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "Distribuidora Norte S.A.",
                    CUIT = "30-70543218-9",
                    Direccion = "Av. Libertador 3200",
                    Localidad = "San Miguel de Tucumán",
                    Provincia = "Tucumán",
                    CodigoPostal = "4000",
                    Telefono = "381-421-5566",
                    Email = "info@distnorte.com",
                    FechaAlta = new DateTime(2025, 7, 1),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "Ana Lucía Fernández",
                    DNI = "25987654",
                    Direccion = "Mitre 1122",
                    Localidad = "Mar del Plata",
                    Provincia = "Buenos Aires",
                    CodigoPostal = "7600",
                    Telefono = "223-491-3344",
                    Email = "analucia.f@outlook.com",
                    FechaAlta = new DateTime(2025, 7, 18),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "Comercial del Litoral S.R.L.",
                    CUIT = "30-68765432-1",
                    Direccion = "Urquiza 567",
                    Localidad = "Santa Fe",
                    Provincia = "Santa Fe",
                    CodigoPostal = "3000",
                    Telefono = "342-455-7788",
                    Email = "administracion@litoral.com.ar",
                    FechaAlta = new DateTime(2025, 8, 5),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "Roberto Díaz",
                    DNI = "18765432",
                    Direccion = "Alsina 234",
                    Localidad = "Bahía Blanca",
                    Provincia = "Buenos Aires",
                    CodigoPostal = "8000",
                    Telefono = "291-455-2211",
                    Email = "roberto.diaz@yahoo.com",
                    FechaAlta = new DateTime(2025, 8, 22),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "Servicios Integrales Patagonia",
                    CUIT = "30-72109876-5",
                    Direccion = "Av. San Martín 1500",
                    Localidad = "Neuquén",
                    Provincia = "Neuquén",
                    CodigoPostal = "8300",
                    Telefono = "299-448-9900",
                    Email = "contacto@patagonia-serv.com",
                    FechaAlta = new DateTime(2025, 9, 1),
                    Activo = true
                },
                new Cliente
                {
                    Nombre = "Laura Beatriz Gómez",
                    DNI = "30567890",
                    Direccion = "Rivadavia 890",
                    Localidad = "Salta",
                    Provincia = "Salta",
                    CodigoPostal = "4400",
                    Telefono = "387-421-1122",
                    Email = "laura.gomez@hotmail.com",
                    FechaAlta = new DateTime(2025, 9, 10),
                    Activo = false   // Baja lógica de ejemplo
                }
            };

            context.Clientes.AddRange(clientes);
            context.SaveChanges();

            // =====================
            // PRODUCTOS
            // =====================
            var productos = new Producto[]
            {
                new Producto { Codigo = "NOTE-001", Descripcion = "Notebook 15.6\" Intel i5 16GB RAM 512GB SSD", Precio = 850000m, IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 3, 15) },
                new Producto { Codigo = "MOUSE-02", Descripcion = "Mouse inalámbrico ergonómico",                 Precio = 18500m,  IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 3, 15) },
                new Producto { Codigo = "TECL-03",  Descripcion = "Teclado mecánico RGB switch blue",             Precio = 42500m,  IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 3, 20) },
                new Producto { Codigo = "MONI-04",  Descripcion = "Monitor LED 27\" Full HD 75Hz",                Precio = 210000m, IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 4, 1) },
                new Producto { Codigo = "AURI-05",  Descripcion = "Auriculares Bluetooth con micrófono",          Precio = 32000m,  IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 4, 10) },
                new Producto { Codigo = "IMP-06",   Descripcion = "Impresora multifunción láser WiFi",             Precio = 185000m, IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 5, 5) },
                new Producto { Codigo = "CABL-07",  Descripcion = "Cable HDMI 2.0 2 metros",                       Precio = 6500m,   IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 5, 12) },
                new Producto { Codigo = "WEBC-08",  Descripcion = "Webcam Full HD 1080p con micrófono",            Precio = 45000m,  IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 6, 1) },
                new Producto { Codigo = "SSD-09",   Descripcion = "Disco SSD 1TB NVMe M.2",                        Precio = 98000m,  IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 6, 15) },
                new Producto { Codigo = "RAM-10",   Descripcion = "Memoria RAM DDR4 16GB 3200MHz",                 Precio = 72000m,  IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 7, 1) },
                new Producto { Codigo = "SILL-11",  Descripcion = "Silla gamer ergonómica con apoyo lumbar",       Precio = 145000m, IVA = 21m, Activo = true,  FechaAlta = new DateTime(2025, 7, 20) },
                new Producto { Codigo = "MOUSE-12", Descripcion = "Mouse pad XL gamer RGB",                        Precio = 12500m,  IVA = 21m, Activo = false, FechaAlta = new DateTime(2025, 8, 1) }
            };

            context.Productos.AddRange(productos);
            context.SaveChanges();

            // =====================
            // FACTURAS + DETALLES
            // =====================

            // Factura 1
            var factura1 = new Factura
            {
                ClienteId = clientes[0].Id,
                FechaEmision = new DateTime(2025, 9, 5, 10, 30, 0),
                Numero = "00000001",
                Nota = "Entrega en oficina central",
                MetodoPago = "Tarjeta de crédito",
                Activa = true,
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        ProductoId = productos[0].Id,
                        Descripcion = productos[0].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[0].Precio,
                        IVA = productos[0].IVA,
                        Subtotal = productos[0].Precio * 1,
                        ImporteIVA = productos[0].Precio * 1 * productos[0].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[1].Id,
                        Descripcion = productos[1].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[1].Precio,
                        IVA = productos[1].IVA,
                        Subtotal = productos[1].Precio * 1,
                        ImporteIVA = productos[1].Precio * 1 * productos[1].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[2].Id,
                        Descripcion = productos[2].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[2].Precio,
                        IVA = productos[2].IVA,
                        Subtotal = productos[2].Precio * 1,
                        ImporteIVA = productos[2].Precio * 1 * productos[2].IVA / 100
                    }
                }
            };
            factura1.Subtotal = factura1.Detalles.Sum(d => d.Subtotal);
            factura1.TotalIVA = factura1.Detalles.Sum(d => d.ImporteIVA);
            factura1.Total = factura1.Subtotal + factura1.TotalIVA;

            // Factura 2
            var factura2 = new Factura
            {
                ClienteId = clientes[1].Id,
                FechaEmision = new DateTime(2025, 9, 12, 15, 45, 0),
                Numero = "00000002",
                MetodoPago = "Efectivo",
                Activa = true,
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        ProductoId = productos[3].Id,
                        Descripcion = productos[3].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[3].Precio,
                        IVA = productos[3].IVA,
                        Subtotal = productos[3].Precio,
                        ImporteIVA = productos[3].Precio * productos[3].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[4].Id,
                        Descripcion = productos[4].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[4].Precio,
                        IVA = productos[4].IVA,
                        Subtotal = productos[4].Precio,
                        ImporteIVA = productos[4].Precio * productos[4].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[6].Id,
                        Descripcion = productos[6].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[6].Precio,
                        IVA = productos[6].IVA,
                        Subtotal = productos[6].Precio,
                        ImporteIVA = productos[6].Precio * productos[6].IVA / 100
                    }
                }
            };
            factura2.Subtotal = factura2.Detalles.Sum(d => d.Subtotal);
            factura2.TotalIVA = factura2.Detalles.Sum(d => d.ImporteIVA);
            factura2.Total = factura2.Subtotal + factura2.TotalIVA;

            // Factura 3
            var factura3 = new Factura
            {
                ClienteId = clientes[2].Id,
                FechaEmision = new DateTime(2025, 9, 18, 11, 20, 0),
                Numero = "00000003",
                Nota = "Pedido urgente - priorizar envío",
                MetodoPago = "Pago con QR",
                Activa = true,
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        ProductoId = productos[5].Id,
                        Descripcion = productos[5].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[5].Precio,
                        IVA = productos[5].IVA,
                        Subtotal = productos[5].Precio,
                        ImporteIVA = productos[5].Precio * productos[5].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[7].Id,
                        Descripcion = productos[7].Descripcion,
                        Cantidad = 2,
                        PrecioUnitario = productos[7].Precio,
                        IVA = productos[7].IVA,
                        Subtotal = productos[7].Precio * 2,
                        ImporteIVA = productos[7].Precio * 2 * productos[7].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[8].Id,
                        Descripcion = productos[8].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[8].Precio,
                        IVA = productos[8].IVA,
                        Subtotal = productos[8].Precio,
                        ImporteIVA = productos[8].Precio * productos[8].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[9].Id,
                        Descripcion = productos[9].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[9].Precio,
                        IVA = productos[9].IVA,
                        Subtotal = productos[9].Precio,
                        ImporteIVA = productos[9].Precio * productos[9].IVA / 100
                    }
                }
            };
            factura3.Subtotal = factura3.Detalles.Sum(d => d.Subtotal);
            factura3.TotalIVA = factura3.Detalles.Sum(d => d.ImporteIVA);
            factura3.Total = factura3.Subtotal + factura3.TotalIVA;

            // Factura 4 (Anulada)
            var factura4 = new Factura
            {
                ClienteId = clientes[3].Id,
                FechaEmision = new DateTime(2025, 9, 20, 9, 10, 0),
                Numero = "00000004",
                Nota = "Cliente solicitó anulación por error de pedido",
                MetodoPago = "Tarjeta de débito",
                Activa = false,
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        ProductoId = productos[1].Id,
                        Descripcion = productos[1].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[1].Precio,
                        IVA = productos[1].IVA,
                        Subtotal = productos[1].Precio,
                        ImporteIVA = productos[1].Precio * productos[1].IVA / 100
                    }
                }
            };
            factura4.Subtotal = factura4.Detalles.Sum(d => d.Subtotal);
            factura4.TotalIVA = factura4.Detalles.Sum(d => d.ImporteIVA);
            factura4.Total = factura4.Subtotal + factura4.TotalIVA;

            // Factura 5
            var factura5 = new Factura
            {
                ClienteId = clientes[4].Id,
                FechaEmision = new DateTime(2025, 9, 25, 16, 0, 0),
                Numero = "00000005",
                Nota = "Factura a 30 días",
                MetodoPago = "Tarjeta de crédito",
                Activa = true,
                Detalles = new List<DetalleFactura>
                {
                    new DetalleFactura
                    {
                        ProductoId = productos[10].Id,
                        Descripcion = productos[10].Descripcion,
                        Cantidad = 2,
                        PrecioUnitario = productos[10].Precio,
                        IVA = productos[10].IVA,
                        Subtotal = productos[10].Precio * 2,
                        ImporteIVA = productos[10].Precio * 2 * productos[10].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[3].Id,
                        Descripcion = productos[3].Descripcion,
                        Cantidad = 1,
                        PrecioUnitario = productos[3].Precio,
                        IVA = productos[3].IVA,
                        Subtotal = productos[3].Precio,
                        ImporteIVA = productos[3].Precio * productos[3].IVA / 100
                    },
                    new DetalleFactura
                    {
                        ProductoId = productos[6].Id,
                        Descripcion = productos[6].Descripcion,
                        Cantidad = 0.5m,
                        PrecioUnitario = productos[6].Precio,
                        IVA = productos[6].IVA,
                        Subtotal = productos[6].Precio * 0.5m,
                        ImporteIVA = productos[6].Precio * 0.5m * productos[6].IVA / 100
                    }
                }
            };
            factura5.Subtotal = factura5.Detalles.Sum(d => d.Subtotal);
            factura5.TotalIVA = factura5.Detalles.Sum(d => d.ImporteIVA);
            factura5.Total = factura5.Subtotal + factura5.TotalIVA;

            context.Facturas.AddRange(factura1, factura2, factura3, factura4, factura5);
            context.SaveChanges();
        }
    }
}