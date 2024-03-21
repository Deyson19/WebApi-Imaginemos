
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Services.Contract
{
    public interface IProductosService
    {
        Task<ResponseDto<IEnumerable<Producto>>> GetAll();
        Task<ResponseDto<Producto>> GetById(int id);
        Task<ResponseDto<List<Producto>>> Search(string name);
        Task<ResponseDto<Producto>> Add(Producto modelo);
        Task<ResponseDto<Producto>> Update(Producto update);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
