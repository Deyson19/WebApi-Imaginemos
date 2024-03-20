namespace WebApi_Imaginemos_DTOs
{
    public class VentaDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioDto Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
