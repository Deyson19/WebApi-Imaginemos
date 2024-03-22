
using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos.Services.Implementation;

namespace WebApi_Imaginemos.TestServices
{
    [TestClass]
    public class DetalleVentaService_Test
    {
        private IDetalleVentasService _detalleVentasService;
        private ImaginemosDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {

            var options = new DbContextOptions<ImaginemosDbContext>();
            _dbContext = new ImaginemosDbContext(options);
            _detalleVentasService = new DetalleVentasService(_dbContext);
        }

        [TestMethod]
        public async Task GetById_ExistDetalleVenta()
        {
            // Arrange
            int id = 10;

            // Act
            var response = await _detalleVentasService.GetById(id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task GetById_NonExistDetalleVenta()
        {
            // Arrange
            int id = 100;

            // Act
            var response = await _detalleVentasService.GetById(id);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Modelo);
        }
        [TestMethod]
        public async Task Delete_ExistDetalleVenta_ReturnOK()
        {
            //el id debe existir en la tabla
            // Arrange
            int id = 8;

            // Act
            var response = await _detalleVentasService.Delete(id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsTrue(response.Modelo);
        }

        [TestMethod]
        public async Task Delete_NotExistDetalleVenta_ReturnsFail()
        {
            // Arrange
            int id = 7;
            // Act
            var response = await _detalleVentasService.Delete(id);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsFalse(response.Modelo);
        }
        [TestMethod]
        public async Task Add_ValidDetalleVenta_ReturnsSuccessResponse()
        {
            // Arrange
            var newDetalleVenta = new DetalleVenta { PrecioUnitario = 2300, Cantidad = 5, ProductoId = 4, Total = 2300 * 5, VentaId = 3 };

            // Act
            var response = await _detalleVentasService.Add(newDetalleVenta);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(newDetalleVenta, response.Modelo);
        }

        [TestMethod]
        public async Task Add_InvalidDetalleVenta_ReturnsFailureResponse()
        {
            // Arrange
            DetalleVenta newDetalleVenta = null;

            // Act
            var response = await _detalleVentasService.Add(newDetalleVenta);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Modelo);
        }
        [TestMethod]
        public async Task Update_ValidDetalleVenta()
        {
            // Arrange
            var updateDetalleVenta = new DetalleVenta
            {
                Id = 36,
                ProductoId = 8,
                VentaId = 36,
                PrecioUnitario = 30,
                Cantidad = 5,
                Total = 5 * 30
            };
            // Act
            var response = await _detalleVentasService.Update(updateDetalleVenta);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(updateDetalleVenta, response.Modelo);
        }

        [TestMethod]
        public async Task Update_InvalidDetalleVenta()
        {
            // Arrange
            var updateDetalleVenta = new DetalleVenta
            {
                Id = 3,
                ProductoId = 8,
                VentaId = 36,
                PrecioUnitario = 30,
                Cantidad = 5,
                Total = 5 * 30
            };

            // Act
            var response = await _detalleVentasService.Update(updateDetalleVenta);

            // Assert
            Assert.IsFalse(response.IsSuccess);
        }
    }
}