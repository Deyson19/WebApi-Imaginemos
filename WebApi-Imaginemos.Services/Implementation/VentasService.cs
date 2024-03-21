
using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Services.Implementation
{
    public class VentasService(ImaginemosDbContext dbContext) : IVentasService
    {
        private readonly ImaginemosDbContext _dbContext = dbContext;

        public async Task<ResponseDto<Venta_Detalle_Usuario>> Add(RegistrarVenta newSale)
        {
            //obtener todos los productos actuales para poder navegar hacia su precio
            var productsList = await _dbContext.Producto.ToListAsync();
            //obtener informacion del usuario
            var userInfo = await _dbContext.Usuario.FirstOrDefaultAsync(x => x.Id == newSale.UsuarioId);
            //crear objetos para asignar luego de guardar los datos y armar posteriormente la respuesta
            var detalleVenta = new DetalleVenta();
            var ventaUnica = new Venta();

            foreach (var items in newSale.Productos)
            {
                var productSelected = productsList.Where(x => x.Id == items.ProductoId).SingleOrDefault();
                var singleSale = new Venta
                {
                    UsuarioId = newSale.UsuarioId,
                    Total = productSelected.Precio * items.Cantidad
                };
                var addSale = await _dbContext.Venta.AddAsync(singleSale);


                var saleDetails = new DetalleVenta
                {
                    ProductoId = productSelected.Id,
                    VentaId = addSale.Entity.Id,
                    Total = singleSale.Total,
                    PrecioUnitario = productSelected.Precio
                };
                await _dbContext.DetalleDeVenta.AddAsync(saleDetails);

                //asignar los valores para armar la respuesta de la tarea
                ventaUnica = singleSale;
                detalleVenta = saleDetails;
            }
            await _dbContext.SaveChangesAsync();
            return new ResponseDto<Venta_Detalle_Usuario>
            {
                IsSuccess = true,
                Modelo = new Venta_Detalle_Usuario
                {
                    Id = detalleVenta.Id,
                    VentaDetalleId = detalleVenta.Id,
                    UsuarioId = ventaUnica.UsuarioId,
                    NombreUsuario = userInfo!.Nombre,
                    Fecha = DateTime.Now,
                    Total = detalleVenta.Total
                }
            };
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var findSale = await GetSale(id);
            if (findSale != null)
            {
                _dbContext.Venta.Remove(findSale);
                await _dbContext.SaveChangesAsync();
                return new ResponseDto<bool> { IsSuccess = true, Modelo = true };
            }
            return new ResponseDto<bool>
            {
                IsSuccess = false,
                Modelo = false
            };
        }

        public async Task<ResponseDto<IEnumerable<Venta>>> GetAll()
        {
            var sales = await _dbContext.Venta.ToListAsync();

            return new ResponseDto<IEnumerable<Venta>> { IsSuccess = sales.Any(), Modelo = sales };
        }

        public async Task<ResponseDto<Venta>> GetById(int id)
        {
            var findSale = await GetSale(id);
            return new ResponseDto<Venta>
            {
                IsSuccess = findSale != null,
                Modelo = findSale
            };
        }

        public async Task<ResponseDto<List<Venta>>> Search(DateTime initialDate, DateTime endDate, string? name, string? dni)
        {
            var searchSale = await _dbContext.Venta.Include(z => z.Usuario).Where(
                x => x.Fecha >= initialDate && x.Fecha <= endDate
                || x.Usuario.Nombre.Contains(name) || x.Usuario.DNI.Contains(dni)
                ).ToListAsync();
            return new ResponseDto<List<Venta>>
            {
                IsSuccess = searchSale != null,
                Modelo = searchSale
            };
        }

        public async Task<ResponseDto<Venta>> Update(VentaUsuarioDto updateSale)
        {
            var findSale = await GetSale(updateSale.Id);
            if (findSale != null)
            {
                findSale.Total = updateSale.Total;
                findSale.UsuarioId = updateSale.UsuarioId;
                findSale.Fecha = updateSale.Fecha;

                _dbContext.Venta.Update(findSale);
                await _dbContext.SaveChangesAsync();

                return new ResponseDto<Venta>
                {
                    IsSuccess = true,
                    Modelo = findSale
                };
            }
            return new ResponseDto<Venta>
            {
                IsSuccess = false,
                Modelo = findSale
            };
        }

        private async Task<Venta> GetSale(int id)
        {
            return await _dbContext.Venta.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
