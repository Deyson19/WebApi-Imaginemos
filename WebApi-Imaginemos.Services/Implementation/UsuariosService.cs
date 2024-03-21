
using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Services.Implementation
{
    public class UsuariosService(ImaginemosDbContext dbContext) : IUsuariosService
    {
        private readonly ImaginemosDbContext _dbContext = dbContext;

        public async Task<ResponseDto<Usuario>> Add(Usuario newUser)
        {
            var addUser = await _dbContext.Usuario.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            return new ResponseDto<Usuario>
            {
                IsSuccess = addUser.Entity.Id != 0,
                Modelo = newUser
            };
        }

        public async Task<ResponseDto<bool>> Delete(int id)
        {
            var findUser = await obtenerUsuario(id);

            if (findUser != null)
            {
                _dbContext.Usuario.Remove(findUser);
                await _dbContext.SaveChangesAsync();
                return new ResponseDto<bool>
                {
                    IsSuccess = true,
                    Modelo = true,
                };
            }
            return new ResponseDto<bool>
            {
                IsSuccess = false,
                Modelo = false
            };
        }

        public async Task<ResponseDto<IEnumerable<Usuario>>> GetAll()
        {
            var users = await _dbContext.Usuario.ToListAsync();
            return new ResponseDto<IEnumerable<Usuario>>
            {
                IsSuccess = users.Any(),
                Modelo = users
            };
        }

        public async Task<ResponseDto<Usuario>> GetById(int id)
        {
            var user = new Usuario();
            var findUser = await obtenerUsuario(id);
            if (findUser != null)
            {
                user = findUser;
                return new ResponseDto<Usuario>
                {
                    IsSuccess = true,
                    Modelo = user
                };
            }
            return new ResponseDto<Usuario>
            {
                IsSuccess = false,
                Modelo = user
            };
        }

        public async Task<ResponseDto<List<Usuario>>> GetByNameOrDni(string? name, string? dni)
        {
            var user = new List<Usuario>();

            if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(dni))
            {
                var findUsers = await _dbContext.Usuario.Where(x => x.Nombre.Contains(name) || x.DNI.Contains(dni)).ToListAsync();
                if (findUsers.Any())
                {
                    user = findUsers;
                }
            }
            return new ResponseDto<List<Usuario>>
            {
                IsSuccess = true,
                Modelo = user
            };
        }

        public async Task<ResponseDto<Usuario>> Update(Usuario updateUser)
        {
            var findUser = await obtenerUsuario(updateUser.Id);

            if (findUser != null)
            {
                findUser.Nombre = updateUser.Nombre;
                findUser.DNI = updateUser.DNI;

                _dbContext.Usuario.Update(findUser);
                await _dbContext.SaveChangesAsync();
                return new ResponseDto<Usuario> { IsSuccess = true, Modelo = updateUser };
            }
            return new ResponseDto<Usuario> { IsSuccess = false, Modelo = findUser };
        }


        private async Task<Usuario> obtenerUsuario(int id)
        {
            var findUser = await _dbContext.Usuario.FirstOrDefaultAsync(x => x.Id == id);
            return findUser;
        }
    }
}
