using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Services.Implementation
{
    public class DetalleVentasService(ImaginemosDbContext dbContext) : IDetalleVentasService
    {
        private readonly ImaginemosDbContext _dbContext = dbContext;

        public async Task<ResponseDto<DetalleVenta>> Add(DetalleVenta newDetail)
        {
            var saveDetail = _dbContext.DetalleDeVenta.Add(newDetail);
            await _dbContext.SaveChangesAsync();
            return new ResponseDto<DetalleVenta>
            {
                IsSuccess = saveDetail.Entity != null,
                Modelo = saveDetail.Entity
            };
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var findDetail = await DetalleVenta(id);
            if (findDetail != null)
            {
                _dbContext.DetalleDeVenta.Remove(findDetail);
                await _dbContext.SaveChangesAsync();
            }
            return new ResponseDto<bool>
            {
                IsSuccess = findDetail != null,
                Modelo = findDetail != null
            };
        }

        public async Task<ResponseDto<IEnumerable<DetalleVenta>>> GetAll(int ventaId)
        {
            var salesDetails = await _dbContext.DetalleDeVenta.Where(x=>x.VentaId == ventaId).ToListAsync();
            return new ResponseDto<IEnumerable<DetalleVenta>>
            {
                IsSuccess = salesDetails.Count != 0,
                Modelo = salesDetails
            };
        }

        public async Task<ResponseDto<DetalleVenta>> GetById(int id)
        {
            return new ResponseDto<DetalleVenta>
            {
                IsSuccess = await DetalleVenta(id) != null,
                Modelo = await DetalleVenta(id)
            };
        }

        public async Task<ResponseDto<DetalleVenta>> Update(DetalleVenta update)
        {
            var findDetail = await DetalleVenta(update.Id);
            if (findDetail != null)
            {
                _dbContext.DetalleDeVenta.Update(update);
                await _dbContext.SaveChangesAsync();
            }
            return new ResponseDto<DetalleVenta>
            {
                IsSuccess = findDetail != null && update != null,
                Modelo = update
            };
        }

        private async Task<DetalleVenta> DetalleVenta(int id)
        {
            return await _dbContext.DetalleDeVenta.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
