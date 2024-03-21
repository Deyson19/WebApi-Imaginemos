
using System.ComponentModel.DataAnnotations.Schema;

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
        public decimal Total { get; set; }
    }
}
