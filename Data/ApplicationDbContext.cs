
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacion.Models;

namespace SistemaFacturacion.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ==========================================
        // TABLAS DEL SISTEMA DE FACTURACIÓN
        // ==========================================

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Factura> Facturas { get; set; }

        public DbSet<DetalleFactura> DetallesFactura { get; set; }

        // ==========================================
        // CONFIGURACIÓN DE ENTIDADES
        // ==========================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ======================================
            // PRODUCTOS
            // ======================================

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Producto>()
                .Property(p => p.IVA)
                .HasPrecision(5, 2);

            // ======================================
            // FACTURAS
            // ======================================

            modelBuilder.Entity<Factura>()
                .Property(f => f.Subtotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Factura>()
                .Property(f => f.TotalIVA)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Factura>()
                .Property(f => f.Total)
                .HasPrecision(18, 2);


            // ======================================
            // DETALLES DE FACTURA
            // ======================================

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.Cantidad)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.IVA)
                .HasPrecision(5, 2);

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.ImporteIVA)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DetalleFactura>()
                .Property(d => d.Subtotal)
                .HasPrecision(18, 2);


        }
    }
}
