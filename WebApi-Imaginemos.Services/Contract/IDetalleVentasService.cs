

using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Services.Contract
{
    public interface IDetalleVentasService
    {
        Task<ResponseDto<IEnumerable<DetalleVenta>>> GetAll(int ventaId);
        Task<ResponseDto<IEnumerable<DetalleVenta>>> GetAll();
        Task<ResponseDto<DetalleVenta>> GetById(int id);
        Task<ResponseDto<DetalleVenta>> Add(DetalleVenta newDetail);
        Task<ResponseDto<DetalleVenta>> Update(DetalleVenta update);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
