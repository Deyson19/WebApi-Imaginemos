using AutoMapper;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos_DTOs;

namespace WebApi_Imaginemos.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Usuario,UsuarioDto>().ReverseMap();
            CreateMap<Venta,VentaDto>().ReverseMap();
            CreateMap<DetalleVenta,DetalleVentaDto>().ReverseMap();
            CreateMap<Producto,ProductoDto>().ReverseMap();
        }
    }
}
