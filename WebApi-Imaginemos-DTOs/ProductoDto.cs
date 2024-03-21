namespace WebApi_Imaginemos_DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
    }
    public class ProductoDto_Crear
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
    }
}
