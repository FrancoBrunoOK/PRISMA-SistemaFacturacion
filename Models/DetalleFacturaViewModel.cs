namespace SistemaFacturacion.Models
{
    public class DetalleFacturaViewModel
    {
        public int ProductoId { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public decimal PrecioUnitario { get; set; }

        public decimal Cantidad { get; set; } = 1;

        public decimal IVA { get; set; }

        public decimal Subtotal
        {
            get
            {
                return PrecioUnitario * Cantidad;
            }
        }

        public decimal ImporteIVA
        {
            get
            {
                return Subtotal * IVA / 100;
            }
        }
    }
}