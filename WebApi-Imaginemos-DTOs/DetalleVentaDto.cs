
namespace WebApi_Imaginemos_DTOs
{
    public class DetalleVentaDto
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
