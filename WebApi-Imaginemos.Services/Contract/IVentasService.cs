

using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Services.Contract
{
    public interface IVentasService
    {
        Task<ResponseDto<IEnumerable<Venta>>> GetAll();
        Task<ResponseDto<Venta>> GetById(int id);
        Task<ResponseDto<List<Venta>>> Search(DateTime initialDate, DateTime endDate, string? name, string? dni);
        Task<ResponseDto<VentaDetalleUsuario>> Add(RegistrarVenta newSale);
        Task<ResponseDto<Venta>> Update(VentaUsuarioDto updateSale);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
