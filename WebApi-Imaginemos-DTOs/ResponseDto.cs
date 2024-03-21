
namespace WebApi_Imaginemos_DTOs
{
    //respuestas personalizadas desde los servicios
    public class ResponseDto<T> 
    {
        public bool IsSuccess { get; set; }
        public T Modelo { get; set; }
    }
}
