
using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.Entities;

namespace WebApi_Imaginemos.DataAccess
{
    public class ImaginemosDbContext : DbContext
    {
        public ImaginemosDbContext(DbContextOptions<ImaginemosDbContext> op) :base(op)
        {
            
        }
        string connectionString = "Host=localhost; Database=WebApi-Imaginemos; Username=postgres; Password=deyson.dev";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }
        public virtual DbSet<DetalleVenta> DetalleDeVenta { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
    }
}
