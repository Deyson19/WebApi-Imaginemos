
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi_Imaginemos.Entities
{
    public class Venta
    {
        public int Id { get; set; }
        [ForeignKey("UsuarioId")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }
        [JsonIgnore]
        public ICollection<DetalleVenta> DetalleVentas { get; set; }
        public decimal Total { get; set; }
    }
}
