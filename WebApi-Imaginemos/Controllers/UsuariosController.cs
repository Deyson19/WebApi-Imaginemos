using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController(IUsuariosService usuariosService, IMapper mapper) : ControllerBase
    {
        private readonly IUsuariosService _usuarioService = usuariosService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> Get(string name, string dni)
        {
            var users = new List<UsuarioDto>();
            if (name != null || dni != null)
            {
                var listUsuarios = await _usuarioService.GetByNameOrDni(name, dni);
                if (listUsuarios.IsSuccess)
                {
                    users = _mapper.Map<List<UsuarioDto>>(listUsuarios.Modelo);
                }
            }
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = new UsuarioDto();
            var findUser = await _usuarioService.GetById(id);
            if (findUser.IsSuccess)
            {
                user = _mapper.Map<UsuarioDto>(findUser.Modelo);
            }
            return Ok(user);
        }
        //* TODO: El post se debe hacer luego de crear el controlador de ventas


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UsuarioDto model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var mappUserDtoToEntity = _mapper.Map<Usuario>(model);
                var updateUserService = await _usuarioService.Update(mappUserDtoToEntity);
                if (updateUserService.IsSuccess)
                {
                    return Ok(model);
                }
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var removeUser = await _usuarioService.Delete(id);
                if (removeUser.IsSuccess)
                {
                    return Ok(removeUser.Modelo);
                }
                return NotFound();
            }
            return BadRequest();
        }
    }

}
