
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
            if (newSale.Usuario is null && newSale.DNI is null && newSale.Productos.Count() == 0)
            {
                return new ResponseDto<VentaDetalleUsuario>
                {
                    IsSuccess = false,
                    Modelo = new VentaDetalleUsuario
                    {
                        Id = 0,
                        VentaDetalleId = [0],
                        UsuarioId = currentUser.Id,
                        NombreUsuario = newSale.Usuario,
                        Fecha = DateTime.Now,
                        Total = 0
                    }
                };
            }
            //crear objetos para asignar luego de guardar los datos y armar posteriormente la respuesta
            var detalleVenta = new DetalleVenta();
            var ventaUnica = new Venta();

            decimal totalVentaResponse = 0;

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
            //asignar el id al usuario creado o existente
            ventaUnica.UsuarioId = currentUser!.Id;
            //guardar la venta realizada
            ventaUnica.Total = 0;
            ventaUnica = await SaveSale(ventaUnica);

            var listaDetalleVenta = new List<DetalleVenta>();
            foreach (var items in newSale.Productos)
            {
                var productSelected = _dbContext.Producto.Where(x => x.Id == items.ProductoId).SingleOrDefault();
                decimal totalVenta = productSelected!.Precio * items.Cantidad;

                totalVentaResponse += ventaUnica.Total;

                //guardar detalles de la venta
                listaDetalleVenta.Add(new DetalleVenta
                {
                    ProductoId = productSelected.Id,
                    PrecioUnitario = productSelected.Precio,
                    Cantidad = items.Cantidad,
                    VentaId = ventaUnica.Id,
                    Total = totalVenta,
                });

            }
            SaveDetailsSale(listaDetalleVenta);
            //manejo de la colección para acceder a los precios y total para el detalle de venta y la venta en si misma
            var x = _dbContext.Venta.Include(x => x.DetalleVentas).Where(x => x.Id == ventaUnica.Id);
            var totalSum = x.FirstOrDefault().DetalleVentas.Sum(z => z.PrecioUnitario * z.Cantidad);
            ventaUnica.Total = totalSum;
            _dbContext.Venta.Update(ventaUnica);
            await _dbContext.SaveChangesAsync();

            return new ResponseDto<VentaDetalleUsuario>
            {
                IsSuccess = true,
                Modelo = new VentaDetalleUsuario
                {
                    Id = ventaUnica.Id,
                    VentaDetalleId = x.FirstOrDefault().DetalleVentas.Where(x => x.VentaId == ventaUnica.Id).Select(x => x.Id).ToArray(),
                    UsuarioId = currentUser.Id,
                    NombreUsuario = newSale.Usuario,
                    Fecha = DateTime.Now,
                    Total = x.FirstOrDefault().DetalleVentas.Sum(z => z.PrecioUnitario * z.Cantidad),
                }
            };
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            if (id != 0)
            {
                var findSale = await GetSale(id);
                if (findSale != null)
                {
                    _dbContext.Venta.Remove(findSale);
                    await _dbContext.SaveChangesAsync();
                    return new ResponseDto<bool> { IsSuccess = true, Modelo = true };
                }
            }
            return new ResponseDto<bool>
            {
                IsSuccess = false,
                Modelo = false
            };
        }

        public async Task<ResponseDto<IEnumerable<Venta>>> GetAll()
        {
            var sales = await _dbContext.Venta.Include(x => x.Usuario).ToListAsync();


            return new ResponseDto<IEnumerable<Venta>> { IsSuccess = sales.Any(), Modelo = sales };
        }

        public async Task<ResponseDto<Venta>> GetById(int id)
        {
            if (id != 0)
            {
                var findSale = await GetSale(id);
                return new ResponseDto<Venta>
                {
                    IsSuccess = findSale != null,
                    Modelo = findSale
                };
            }
            return new ResponseDto<Venta>
            {
                IsSuccess = false,
                Modelo = null
            };

        }

        public async Task<ResponseDto<List<Venta>>> Search(DateTime initialDate, DateTime endDate, string name, string dni)
        {
            if (name != null || dni != null)
            {
                var searchSale = await _dbContext.Venta.Include(z => z.Usuario).Where(
                                x => x.Fecha >= initialDate && x.Fecha <= endDate
                                || x.Usuario.Nombre.ToLower().Contains(name.ToLower()) || x.Usuario.DNI.ToLower().Contains(dni.ToLower())
                                ).ToListAsync();
                return new ResponseDto<List<Venta>>
                {
                    IsSuccess = searchSale.Any(),
                    Modelo = searchSale
                };
            }
            return new ResponseDto<List<Venta>>
            {
                IsSuccess = false,
                Modelo = new List<Venta>()
            };
        }

        public async Task<ResponseDto<Venta>> Update(VentaUsuarioDto updateSale)
        {
            if (updateSale.Id != 0)
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
            }
            return new ResponseDto<Venta>
            {
                IsSuccess = false,
                Modelo = null
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

        private void SaveDetailsSale(List<DetalleVenta> detalleVenta)
        {
            _dbContext.DetalleDeVenta.AddRange(detalleVenta);
            _dbContext.SaveChanges();

        }
    }
}
