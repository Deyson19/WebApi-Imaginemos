
using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Services.Implementation
{
    public class ProductosService(ImaginemosDbContext dbContext) : IProductosService
    {
        private readonly ImaginemosDbContext _dbContext = dbContext;
        public async Task<ResponseDto<Producto>> Add(Producto modelo)
        {
            if (modelo.Nombre != null && modelo.Precio != null)
            {
                await _dbContext.Producto.AddAsync(modelo);
                await _dbContext.SaveChangesAsync();
                return new ResponseDto<Producto> { IsSuccess = true, Modelo = modelo };
            }
            return new ResponseDto<Producto> { IsSuccess = false, Modelo = modelo };
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var singleProduct = await GetProduct(id);
            if (singleProduct != null)
            {
                _dbContext.Producto.Remove(singleProduct);
                await _dbContext.SaveChangesAsync();
            }
            return new ResponseDto<bool> { IsSuccess = singleProduct != null, Modelo = singleProduct != null };
        }

        public async Task<ResponseDto<IEnumerable<Producto>>> GetAll()
        {
            var listProduct = await _dbContext.Producto.ToListAsync();
            return new ResponseDto<IEnumerable<Producto>>
            {
                IsSuccess = listProduct != null,
                Modelo = listProduct
            };
        }

        public async Task<ResponseDto<Producto>> GetById(int id)
        {
            return new ResponseDto<Producto>
            {
                IsSuccess = await GetProduct(id) != null,
                Modelo = await GetProduct(id)
            };
        }

        public async Task<ResponseDto<List<Producto>>> Search(string name)
        {
            var searchProduct = await _dbContext.Producto.Where(
                x => x.Nombre.ToLower().Contains(name.ToLower())
                ).ToListAsync();
            return new ResponseDto<List<Producto>>
            {
                IsSuccess = searchProduct.Any(),
                Modelo = searchProduct
            };
        }

        public async Task<ResponseDto<Producto>> Update(Producto update)
        {
            var product = await GetProduct(update.Id);
            if (product != null)
            {
                product.Nombre = update.Nombre;
                product.Descripcion = update.Descripcion;
                product.Precio = update.Precio;

                _dbContext.Producto.Update(product);
                await _dbContext.SaveChangesAsync();

            }
            return new ResponseDto<Producto>
            {
                IsSuccess = product != null,
                Modelo = update
            };
        }

        private async Task<Producto> GetProduct(int id) => await _dbContext.Producto.FirstOrDefaultAsync(x => x.Id == id);
    }
}
