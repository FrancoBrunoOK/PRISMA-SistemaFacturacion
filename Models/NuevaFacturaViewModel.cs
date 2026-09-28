namespace SistemaFacturacion.Models
{
    public class NuevaFacturaViewModel
    {
        // Cliente seleccionado
        public int ClienteId { get; set; }

        public string ClienteNombre { get; set; } = string.Empty;

        // Productos agregados a la factura
        public List<DetalleFacturaViewModel> Detalles { get; set; }
            = new List<DetalleFacturaViewModel>();

        // Nota opcional
        public string? Nota { get; set; }

        public string MetodoPago { get; set; } = string.Empty;

        // Subtotal de todos los productos
        public decimal Subtotal
        {
            get
            {
                return Detalles.Sum(d => d.Subtotal);
            }
        }

        // IVA total
        public decimal TotalIVA
        {
            get
            {
                return Detalles.Sum(d => d.ImporteIVA);
            }
        }

        // Total final
        public decimal Total
        {
            get
            {
                return Subtotal + TotalIVA;
            }
        }
    }
}