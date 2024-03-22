using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController(IProductosService productoService, IMapper mapper) : ControllerBase
    {
        private readonly IProductosService _productosService = productoService;
        private readonly IMapper _mapper = mapper;
        private readonly string baseUrl = Helper.baseUrl;

        [HttpGet]
        public async Task<IActionResult> GetProducts(string name)
        {
            var findProducts = await _productosService.Search(name);
            if (findProducts.IsSuccess)
            {
                var productsListDto = _mapper.Map<List<ProductoDto>>(findProducts.Modelo);
                return Ok(productsListDto);
            }
            return NoContent();
        }

        [HttpGet("Pagination/{total}/{pages}")]
        public async Task<IActionResult> GetPageOfProducts([FromRoute] int total, int pages)
        {
            if (total < 0 || pages <= 0)
            {
                return BadRequest("El total de paginas o el rango no pueden ser negativos");
            }

            var productos = await _productosService.GetAll();
            var totalProductos = productos.Modelo.Count();

            if (pages > totalProductos / total)
            {
                return BadRequest("La cantidad de paginas solicitada supera la cantidad real de elementos");
            }

            var saltarItems = pages * total;
            var tomarItems = total;

            if (saltarItems >= totalProductos)
            {
                return NotFound();
            }
            if (saltarItems + total > totalProductos)
            {
                tomarItems = totalProductos - saltarItems;
            }

            var paginationResult = productos.Modelo.Skip(saltarItems).Take(tomarItems).ToList();

            var totalPaginas = (int)Math.Ceiling((double)totalProductos / total);

            var links = new List<Link>();

            for (int i = 1; i <= totalPaginas; i++)
            {
                Link link = new()
                {
                    Href = baseUrl+ Url.Action("GetPageOfProducts", new { total = total, pages = i }),
                    Rel = i == pages ? "self" : "alternate",
                    Title = $"Page: {i}"
                };

                links.Add(link);
            }

            var responsePagination = new
            {
                Total = totalProductos,
                Items = paginationResult,
                Links = links
            };

            return Ok(responsePagination);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            if (id != 0)
            {
                var findProduct = await _productosService.GetById(id);
                if (findProduct.IsSuccess)
                {
                    var productDto = _mapper.Map<ProductoDto>(findProduct.Modelo);
                    return Ok(productDto);
                }
                return NoContent();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductoDto_Crear modelo)
        {
            if (ModelState.IsValid)
            {
                var productDto = new ProductoDto
                {
                    Nombre = modelo.Nombre,
                    Descripcion = modelo.Descripcion,
                    Precio = modelo.Precio,
                };
                var addProduct = await _productosService.Add(_mapper.Map<Producto>(productDto));
                if (addProduct.IsSuccess)
                {
                    //devolver el producto creado pasandolo por el automapper
                    return Ok(_mapper.Map<ProductoDto>(addProduct.Modelo));
                }
                return BadRequest();
            }
            return BadRequest();
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProductoDto modelo)
        {
            if (id != modelo.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var updateProduct = await _productosService.Update(_mapper.Map<Producto>(modelo));
                if (updateProduct.IsSuccess)
                {
                    return Ok(updateProduct.IsSuccess);
                }
                return BadRequest();
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id != 0)
            {
                var deleteProducto = await _productosService.Delete(id);
                if (deleteProducto.IsSuccess)
                {
                    return Ok(deleteProducto.Modelo);
                }
                return BadRequest();
            }
            return BadRequest();
        }

    }
}
