

namespace WebApi_Imaginemos_DTOs
{
    public class Venta_Detalle_Usuario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int VentaDetalleId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
