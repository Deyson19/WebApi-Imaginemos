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
    public class VentaUsuarioDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
    public class RegistrarVenta
    {

        public int UsuarioId { get; set; } //usuario id en los modelos se crea como entero, y nombre como string
        public string DNI { get; set; }
        public List<VentaProducto> Productos { get; set; }
    }
    public class VentaProducto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
