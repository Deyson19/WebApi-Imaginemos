using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos.Services.Implementation;

namespace WebApi_Imaginemos.TestServices
{
    [TestClass]
    public class ProductosService_Test
    {
        private IProductosService _productosService;
        private ImaginemosDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {

            var options = new DbContextOptions<ImaginemosDbContext>();
            _dbContext = new ImaginemosDbContext(options);
            _productosService = new ProductosService(_dbContext);
        }
        [TestMethod]
        public async Task SearchProducts_Products()
        {
            // Arrange
            string name = "toma";
            // Act
            var result = await _productosService.Search(name);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Modelo);
        }

        [TestMethod]
        public async Task SearchProducts_ProductsFail()
        {
            // Arrange
            string name = "El";
            // Act
            var result = await _productosService.Search(name);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Modelo);
        }

        [TestMethod]
        public async Task GetById_ExistProduct()
        {
            // Arrange
            int id = 1;

            // Act
            var response = await _productosService.GetById(id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task GetById_NoExistProduct()
        {
            // Arrange
            int id = 100;

            // Act
            var response = await _productosService.GetById(id);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Modelo);
        }
        [TestMethod]
        public async Task Delete_Success_ReturnOK()
        {
            //id debe existir  en la tabla de productos
            // Arrange
            int id = 36;
            // Act
            var response = await _productosService.Delete(id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsTrue(response.Modelo);
        }

        [TestMethod]
        public async Task Delete_Fail()
        {
            // Arrange
            int id = 34;
            // Act
            var response = await _productosService.Delete(id);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsFalse(response.Modelo);
        }
        [TestMethod]
        public async Task Add_ValidProduct_ReturnsSuccess()
        {
            // Arrange
            var newProduct = new Entities.Producto { Nombre = "Uvas Pasas", Descripcion = "Uvas para compartir en familia", Precio = 150 };

            // Act
            var response = await _productosService.Add(newProduct);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(newProduct, response.Modelo);
        }

        [TestMethod]
        public async Task Add_InvalidProduct_ReturnsFail()
        {
            // Arrange
            var newProduct = new Producto { Nombre = null, Descripcion = null, Precio = decimal.Zero };

            // Act
            var response = await _productosService.Add(newProduct);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Modelo.Nombre);
        }
        [TestMethod]
        public async Task GetAll_ProductsExist()
        {
            // Act
            var response = await _productosService.GetAll();
            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task GetAll_NoProductsExist()
        {
            // Act
            var response = await _productosService.GetAll();

            // Assert
            Assert.IsFalse(!response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task Update_ValidProduct()
        {
            // Arrange
            var updateProduct = new Producto
            {
                Id = 26,
                Nombre = "Wilma Rabat",
                Descripcion = "Tempor consectetur excepteur",
                Precio = 150
            };
            // Act
            var response = await _productosService.Update(updateProduct);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(updateProduct, response.Modelo);
        }

        [TestMethod]
        public async Task Update_InvalidProduct()
        {
            // Arrange
            var updateUser = new Producto
            {
                Id = 150,
                Nombre = "John Doe",
                Descripcion = "123456789",
                Precio = 736
            };

            // Act
            var response = await _productosService.Update(updateUser);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            //Assert.IsNull(response.Modelo);
        }
    }
}