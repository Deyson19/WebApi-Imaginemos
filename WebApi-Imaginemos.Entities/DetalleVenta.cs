using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_Imaginemos.Entities
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        [ForeignKey("ProductoId")]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        [ForeignKey("VentaId")]
        public int VentaId { get; set; }
        public Venta Venta { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
