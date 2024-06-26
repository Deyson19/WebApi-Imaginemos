﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos.Services.Implementation;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController(IVentasService ventasService, IDetalleVentasService detalleVentas, IMapper mapper) : ControllerBase
    {
        private readonly IVentasService _ventasService = ventasService;
        private readonly IDetalleVentasService _detalleVentas = detalleVentas;
        private readonly IMapper _mapper = mapper;

        private readonly string baseUrl = Helper.baseUrl;


        [HttpGet("PaginationSales/{total}/{pages}")]
        public async Task<IActionResult> GetPageOfSales([FromRoute] int total, int pages)
        {
            if (total < 0 || pages <= 0)
            {
                return BadRequest("El total de paginas o el rango no pueden ser negativos");
            }

            var registros = await _ventasService.GetAll();
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
                    Href = baseUrl + Url.Action("GetPageOfSales", new { total = total, pages = i }),
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
        public async Task<IActionResult> Add([FromBody] RegistrarVenta modelo)
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
        public async Task<IActionResult> Update(int id, VentaUsuarioDto modelo)
        {
            if (id != modelo.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var updateSale = await _ventasService.Update(modelo);
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
                return BadRequest();
            }
            var findSale = await _ventasService.Delete(id);
            if (findSale.IsSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        #region DetalleDeVenta

        [HttpGet("PaginationSalesDetails/{total}/{pages}")]
        public async Task<IActionResult> GetPageOfSalesDetails([FromRoute] int total, int pages)
        {
            if (total < 0 || pages <= 0)
            {
                return BadRequest("El total de paginas o el rango no pueden ser negativos");
            }

            var registros = await _detalleVentas.GetAll();
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
                    Href = baseUrl + Url.Action("GetPageOfSalesDetails", new { total = total, pages = i }),
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

        [HttpGet("{ventaId}/detalles")]
        public async Task<IActionResult> DetalleVenta(int ventaId)
        {
            if (ventaId != 0)
            {
                var findDetails = await _detalleVentas.GetAll(ventaId);
                if (findDetails.IsSuccess)
                {
                    var mappDetail = _mapper.Map<List<DetalleVentaDto>>(findDetails.Modelo);
                    return Ok(mappDetail);
                }
                return NoContent();
            }
            return BadRequest();
        }
        [HttpGet("{ventaId}/detalles/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            if (id != 0)
            {
                var findDetailSale = await _detalleVentas.GetById(id);
                if (findDetailSale.IsSuccess)
                {
                    return Ok(_mapper.Map<DetalleVentaDto>(findDetailSale.Modelo));
                }
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("{ventaId}/detalles/{id}")]
        public async Task<IActionResult> Update(int id, DetalleVentaDto modelo)
        {
            if (id != modelo.Id)
            {
                return BadRequest();
            }
            var mappDetailToEntity = _mapper.Map<DetalleVenta>(modelo);
            var updateDetail = await _detalleVentas.Update(mappDetailToEntity);
            if (updateDetail.IsSuccess)
            {
                return Ok(mappDetailToEntity);
            }
            return BadRequest();
        }
        [HttpDelete("{ventaId}/detalles/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var deleteDetail = await _detalleVentas.Delete(id);
                if (deleteDetail.IsSuccess)
                {
                    return Ok(deleteDetail.Modelo);
                }
                return BadRequest();
            }
            return BadRequest();
        }

        #endregion

    }
}
