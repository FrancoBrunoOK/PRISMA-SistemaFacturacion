namespace SistemaFacturacion.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public decimal IVA { get; set; } = 21;

        public bool Activo { get; set; } = true;

        public DateTime FechaAlta { get; set; } = DateTime.Now;
    }
}