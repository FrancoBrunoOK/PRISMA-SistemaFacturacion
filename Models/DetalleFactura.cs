namespace SistemaFacturacion.Models
{
    public class DetalleFactura
    {
        public int Id { get; set; }

        // Factura a la que pertenece
        public int FacturaId { get; set; }

        public Factura? Factura { get; set; }

        // Producto seleccionado
        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }

        // Datos guardados en el momento de facturar
        public string Descripcion { get; set; } = string.Empty;

        public decimal Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal IVA { get; set; }

        public decimal Subtotal { get; set; }

        public decimal ImporteIVA { get; set; }
    }
}