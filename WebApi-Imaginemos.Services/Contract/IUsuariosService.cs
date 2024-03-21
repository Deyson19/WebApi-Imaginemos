
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Services.Contract
{
    public interface IUsuariosService
    {
        Task<ResponseDto<IEnumerable<Usuario>>> GetAll();
        Task<ResponseDto<Usuario>> GetById(int id);
        Task<ResponseDto<List<Usuario>>> GetByNameOrDni(string? name,string? dni);
        Task<ResponseDto<Usuario>> Add(Usuario newUser);
        Task<ResponseDto<Usuario>> Update(Usuario updateUser);
        Task<ResponseDto<bool>> Delete(int id);
    }
}
