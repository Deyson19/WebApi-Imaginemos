
using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.Entities;

namespace WebApi_Imaginemos.DataAccess
{
    public class ImaginemosDbContext : DbContext
    {
        public ImaginemosDbContext(DbContextOptions<ImaginemosDbContext> op) :base(op)
        {
            
        }

        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }
        public virtual DbSet<DetalleVenta> DetalleDeVenta { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
    }
}
