namespace SistemaFacturacion.Models
{
    public class Factura
    {
        public int Id { get; set; }

        // Cliente al que pertenece la factura
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        // Datos de la factura
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        public string Numero { get; set; } = string.Empty;

        // Totales
        public decimal Subtotal { get; set; }

        public decimal TotalIVA { get; set; }

        public decimal Total { get; set; }

        // Nota opcional
        public string? Nota { get; set; }

        //Forma de pago
        public string MetodoPago { get; set; } = string.Empty;

        // Estado de la factura
        public bool Activa { get; set; } = true;

        // Detalles de los productos
        public List<DetalleFactura> Detalles { get; set; } = new List<DetalleFactura>();
    }
}