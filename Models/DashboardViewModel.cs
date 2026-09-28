namespace SistemaFacturacion.Models
{
    public class DashboardViewModel
    {
        public int ClientesActivos { get; set; }

        public int ProductosActivos { get; set; }

        public int FacturasActivas { get; set; }

        public int FacturasAnuladas { get; set; }

        public decimal TotalFacturado { get; set; }
    }
}