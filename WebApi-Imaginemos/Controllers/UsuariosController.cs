using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos.Services.Implementation;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController(IUsuariosService usuariosService, IMapper mapper) : ControllerBase
    {
        private readonly IUsuariosService _usuarioService = usuariosService;
        private readonly IMapper _mapper = mapper;
        private readonly string baseUrl = Helper.baseUrl;


        [HttpGet("Pagination/{total}/{pages}")]
        public async Task<IActionResult> GetPageOfUsers([FromRoute] int total, int pages)
        {
            if (total < 0 || pages <= 0)
            {
                return BadRequest("El total de paginas o el rango no pueden ser negativos");
            }

            var registros = await _usuarioService.GetAll();
            var totalRegistros = registros.Modelo.Count();

            if (pages > totalRegistros / total)
            {
                return BadRequest("La cantidad de paginas solicitada supera la cantidad real de elementos");
            }

            var saltarItems = pages * total;
            var tomarItems = total;

            if (saltarItems >= totalRegistros)
            {
                return NotFound();
            }
            if (saltarItems + total > totalRegistros)
            {
                tomarItems = totalRegistros - saltarItems;
            }

            var paginationResult = registros.Modelo.Skip(saltarItems).Take(tomarItems).ToList();

            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / total);

            var links = new List<Link>();

            for (int i = 1; i <= totalPaginas; i++)
            {
                Link link = new()
                {
                    Href = baseUrl + Url.Action("GetPageOfUsers", new { total = total, pages = i }),
                    Rel = i == pages ? "self" : "alternate",
                    Title = $"Page: {i}"
                };

                links.Add(link);
            }

            var responsePagination = new
            {
                Total = totalRegistros,
                Items = paginationResult,
                Links = links
            };

            return Ok(responsePagination);
        }

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
