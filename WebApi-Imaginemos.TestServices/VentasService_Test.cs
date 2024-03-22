using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos.Services.Implementation;
using WebApi_Imaginemos_DTOs;


namespace WebApi_Imaginemos.TestServices
{
    [TestClass]
    public class VentasService_Test
    {
        private IVentasService _ventasService;
        private ImaginemosDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptions<ImaginemosDbContext>();
            _dbContext = new ImaginemosDbContext(options);
            _ventasService = new VentasService(_dbContext);
        }

        [TestMethod]
        public async Task GetById_ValidId_ReturnsSuccessWithVenta()
        {
            //el id debe existir en la tabla
            // Arrange
            int id = 8;

            // Act
            var response = await _ventasService.GetById(id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task GetById_InvalidId_ReturnsFailWithNullVenta()
        {
            // Arrange
            int id = 0;
            // Act
            var response = await _ventasService.GetById(id);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Modelo);
        }

        [TestMethod]
        public async Task GetAll_ReturnsSuccessWithSales()
        {
            // Act
            var response = await _ventasService.GetAll();

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
            // Agrega más aserciones según tus necesidades
        }

        [TestMethod]
        public async Task GetAll_ReturnsFailEmptySales()
        {
            // Act
            var response = await _ventasService.GetAll();

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsFalse(response.Modelo.Count() < 1);
        }

        [TestMethod]
        public async Task Delete_ValidId_ReturnsSuccess()
        {
            //el id debe existir en la tabla
            // Arrange
            int id = 4;
            // Act
            var response = await _ventasService.Delete(id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsTrue(response.Modelo);

            // Verificar que la venta ya no existe
            var deletedVenta = await _dbContext.Venta.FindAsync(id);
            Assert.IsNull(deletedVenta);
        }

        [TestMethod]
        public async Task Delete_InvalidId_ReturnsFail()
        {
            // Arrange
            int id = 0;

            // Act
            var response = await _ventasService.Delete(id);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsFalse(response.Modelo);
        }

        [TestMethod]
        public async Task Add_NullNewSale_ReturnsSuccessEmptyWithVentaDetalleUsuario()
        {
            // Arrange
            RegistrarVenta newSale = new()
            {
                Usuario = null,
                DNI = null,
                Productos = new List<VentaProducto>()
            };

            // Act
            var response = await _ventasService.Add(newSale);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
            Assert.AreEqual(0, response.Modelo.Id);
        }

        [TestMethod]
        public async Task Add_ValidNewSale()
        {
            string name = "Eliana";
            string dni = "478-96-523";
            var listaVentas = new List<VentaProducto>
            {
                new() {Cantidad = 3, ProductoId =4},
                new() {Cantidad = 4, ProductoId =8},
                new() {Cantidad = 3, ProductoId =4}
            };
            // Arrange
            var newSale = new RegistrarVenta
            {
                Usuario = name,
                DNI = dni,
                Productos = listaVentas
            };

            // Act
            var response = await _ventasService.Add(newSale);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task Search_ValidParameters()
        {
            // Arrange
            DateTime initialDate = new DateTime(2022, 1, 1);
            DateTime endDate = new DateTime(2022, 12, 31);

            string name = "Eliana";
            string dni = "478-96-523";
            // Act
            var response = await _ventasService.Search(initialDate, endDate, name, dni);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task Search_InvalidParameters()
        {
            // Arrange
            DateTime initialDate = new DateTime(2022, 1, 1);
            DateTime endDate = new DateTime(2022, 12, 31);
            string name = null;
            string dni = null;
            //  string name = "John Doe";
            // string dni = "123456789";

            // Act
            var response = await _ventasService.Search(initialDate, endDate, name, dni);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
            Assert.AreEqual(0, response.Modelo.Count);
        }

        [TestMethod]
        public async Task Update_ValidUpdateSale()
        {
            // Arrange
            int ventaId = 7;
            var updateSale = new VentaUsuarioDto
            {
                Id = ventaId,
                UsuarioId = 32,
                Total = 150,
                Fecha = new DateTime(2024, 3, 8)
            };
            // Act
            var response = await _ventasService.Update(updateSale);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
            Assert.AreEqual(ventaId, response.Modelo.Id);
            // Agrega más aserciones según tus necesidades
        }

        [TestMethod]
        public async Task Update_InvalidUpdateSale()
        {
            // Arrange
            int ventaId = 0;
            var updateSale = new VentaUsuarioDto
            {
                Id = ventaId,
                UsuarioId = 32,
                Total = 150,
                Fecha = new DateTime(2024, 3, 8)
            };

            // Act
            var response = await _ventasService.Update(updateSale);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Modelo);
        }

    }

}