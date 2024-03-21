

namespace WebApi_Imaginemos_DTOs
{
    public class VentaDetalleUsuario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int VentaDetalleId { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
