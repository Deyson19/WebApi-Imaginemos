using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            if (id !=0)
            {
                var deleteProducto =await _productosService.Delete(id);
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
