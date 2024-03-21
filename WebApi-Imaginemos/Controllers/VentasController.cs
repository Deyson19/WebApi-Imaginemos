using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController(IVentasService ventasService) : ControllerBase
    {
        private readonly IVentasService _ventasService = ventasService;

        [HttpGet]
        public async Task<IActionResult> GetAll(DateTime initialDate, DateTime endDate, string? name, string? dni)
        {
            var findSales = await _ventasService.Search(initialDate, endDate, name, dni);
            var salesUsersList = new List<VentaUsuarioDto>();
            if (findSales.IsSuccess)
            {
                foreach (var item in findSales.Modelo)
                {
                    var singleSale = new VentaUsuarioDto()
                    {
                        UsuarioId = item.UsuarioId,
                        Fecha = item.Fecha,
                        Id = item.Id,
                        Total = item.Total,
                        NombreUsuario = item.Usuario.Nombre
                    };
                    salesUsersList.Add(singleSale);
                }
                return Ok(salesUsersList);
            }
            return NoContent();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var findSingleSale = await _ventasService.GetById(id);
            var singleSale = new VentaUsuarioDto();

            if (findSingleSale.IsSuccess)
            {
                singleSale.NombreUsuario = findSingleSale.Modelo.Usuario.Nombre;
                singleSale.Id = findSingleSale.Modelo.Id;
                singleSale.Fecha = findSingleSale.Modelo.Fecha;
                singleSale.UsuarioId = findSingleSale.Modelo.UsuarioId;
                singleSale.Total = findSingleSale.Modelo.Total;
                return Ok(singleSale);
            }
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RegistrarVenta modelo)
        {
            if (ModelState.IsValid)
            {
                var addSale = await _ventasService.Add(modelo);
                if (addSale.IsSuccess)
                {
                    return Ok(addSale.Modelo);
                }
                return BadRequest();
            }
            return BadRequest(modelo);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,Venta_Detalle_Usuario modelo)
        {
            if (ModelState.IsValid)
            {
                var saleUserDto = new VentaUsuarioDto
                {
                    UsuarioId = modelo.UsuarioId,
                    NombreUsuario = modelo.NombreUsuario,
                    Total = modelo.Total,
                    Fecha = modelo.Fecha
                };
                var updateSale = await _ventasService.Update(saleUserDto);
                if (updateSale.IsSuccess)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (id != 0)
            {
                var findSale = await _ventasService.Delete(id);
                if (findSale.IsSuccess)
                {
                    return Ok();
                }
                return NoContent();
            }
            return BadRequest();
        }
    }
}
