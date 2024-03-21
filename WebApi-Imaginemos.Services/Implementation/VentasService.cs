
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
        //gestionar el usuario si existe o se ha creado uno nuevo
        Usuario currentUser = new();
        public async Task<ResponseDto<VentaDetalleUsuario>> Add(RegistrarVenta newSale)
        {

            //crear objetos para asignar luego de guardar los datos y armar posteriormente la respuesta
            var detalleVenta = new DetalleVenta();
            var ventaUnica = new Venta();

            decimal totalVentaResponse = 0;
            //obtener todos los productos actuales para poder navegar hacia su precio
            var productsList = await _dbContext.Producto.ToListAsync();

            //obtener usuario por el dni para evitar duplicidad
            var user = await _dbContext.Usuario.FirstOrDefaultAsync(x => x.DNI.ToLower() == newSale.DNI.ToLower());


            //guardar el nuevo usuario que viene si no existe al realizar la venta 
            if (user is null)
            {
                var newUser = new Usuario
                {
                    DNI = newSale.DNI,
                    Nombre = newSale.Usuario
                };
                await _dbContext.Usuario.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();
                //crea el usuario y se asignan a la instancia ya creada para acceder a los valores para la venta
                currentUser = newUser;
            }
            else
            {
                currentUser = user;

            }
            int productsCounter = newSale.Productos.Count();
            
            foreach (var items in newSale.Productos)
            {
                var productSelected = productsList.Where(x => x.Id == items.ProductoId).SingleOrDefault();
                decimal totalVenta = productSelected!.Precio * items.Cantidad;

                //asignar el id al usuario creado o existente
                ventaUnica.UsuarioId = currentUser!.Id;
                //guardar la venta realizada
                ventaUnica.Total = totalVenta;
                ventaUnica = await SaveSale(ventaUnica);
                

                totalVentaResponse += ventaUnica.Total;

                //guardar detalles de la venta
                detalleVenta.ProductoId = productSelected.Id;
                detalleVenta.PrecioUnitario = productSelected.Precio;
                detalleVenta.Cantidad = items.Cantidad;
                detalleVenta.VentaId = ventaUnica.Id;
                detalleVenta.Total = ventaUnica.Total;
                detalleVenta = await SaveDetailsSale(detalleVenta);

                productsCounter--;
                //restablecer el ventaUnica y limpiar los datos
                if (productsCounter >=1)
                {
                    ventaUnica = new Venta();
                    detalleVenta = new DetalleVenta();
                }
                
            }

            return new ResponseDto<VentaDetalleUsuario>
            {
                IsSuccess = true,
                Modelo = new VentaDetalleUsuario
                {
                    Id = detalleVenta.Id,
                    VentaDetalleId = detalleVenta.Id,
                    UsuarioId = currentUser.Id,
                    NombreUsuario = newSale.Usuario,
                    Fecha = DateTime.Now,
                    Total = totalVentaResponse,
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

        public async Task<ResponseDto<List<Venta>>> Search(DateTime initialDate, DateTime endDate, string name, string dni)
        {
            var searchSale = await _dbContext.Venta.Include(z => z.Usuario).Where(
                x => x.Fecha >= initialDate && x.Fecha <= endDate
                || x.Usuario.Nombre.ToLower().Contains(name.ToLower()) || x.Usuario.DNI.ToLower().Contains(dni.ToLower())
                ).ToListAsync();
            return new ResponseDto<List<Venta>>
            {
                IsSuccess = searchSale != null,
                Modelo = searchSale!
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
            return await _dbContext.Venta.Include(x => x.Usuario).FirstOrDefaultAsync(x => x.Id == id);
        }

        private async Task<Venta> SaveSale(Venta venta)
        {
            var newItem = await _dbContext.Venta.AddAsync(venta);
            await _dbContext.SaveChangesAsync();

            return newItem.Entity;
        }

        private async Task<DetalleVenta> SaveDetailsSale(DetalleVenta detalleVenta)
        {
            var newItem = await _dbContext.DetalleDeVenta.AddAsync(detalleVenta);
            await _dbContext.SaveChangesAsync();

            return newItem.Entity;
        }
    }
}
