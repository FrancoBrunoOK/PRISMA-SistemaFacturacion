namespace SistemaFacturacion.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? CUIT { get; set; }

        public string? DNI { get; set; }

        public string? Direccion { get; set; }

        public string? Localidad { get; set; }

        public string? Provincia { get; set; }

        public string? CodigoPostal { get; set; }

        public string? Telefono { get; set; }

        public string? Email { get; set; }

        public DateTime FechaAlta { get; set; } = DateTime.Now;

        public bool Activo { get; set; } = true;

    }
}