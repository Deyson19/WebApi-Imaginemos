using Microsoft.EntityFrameworkCore;
using Moq;
using System.Runtime.CompilerServices;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Entities;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos.Services.Implementation;

namespace WebApi_Imaginemos.TestServices
{
    [TestClass]
    public class UsuariosService_Test
    {
        private IUsuariosService _usuariosService;
        private ImaginemosDbContext _dbContext;

        [TestInitialize]
        public void Setup()
        {

            var options = new DbContextOptions<ImaginemosDbContext>();
            _dbContext = new ImaginemosDbContext(options);
            _usuariosService = new UsuariosService(_dbContext);
        }
        [TestMethod]
        public async Task GetByNameOrDni_ValidNameAndDni()
        {
            // Arrange
            string name = "Eliana";
            string dni = "478-96-523";

            // Act
            var result = await _usuariosService.GetByNameOrDni(name, dni);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Modelo);
            Assert.AreEqual(name, result.Modelo.FirstOrDefault().Nombre);
            Assert.AreEqual(dni, result.Modelo.FirstOrDefault().DNI);
        }

        [TestMethod]
        public async Task GetUsersByNameOrDni_FilterByNameOrDni()
        {
            // Arrange
            string name = "El";
            string dni = "47";

            // Act
            var result = await _usuariosService.GetByNameOrDni(name, dni);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Modelo);
        }

        [TestMethod]
        public async Task GetById_ExistUser()
        {
            // Arrange
            int id = 1;

            // Act
            var response = await _usuariosService.GetById(id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task GetById_NonExistUser()
        {
            // Arrange
            int id = 100;

            // Act
            var response = await _usuariosService.GetById(id);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }
        [TestMethod]
        public async Task Delete_ExistUser_ReturnOK()
        {
            //el id debe existir en la tabla
            // Arrange
            int id = 7;

            // Act
            var response = await _usuariosService.Delete(id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsTrue(response.Modelo);
        }

        [TestMethod]
        public async Task Delete_NotExistUser_ReturnsFail()
        {
            // Arrange
            int id = 34;
            // Act
            var response = await _usuariosService.Delete(id);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsFalse(response.Modelo);
        }
        [TestMethod]
        public async Task Add_ValidUser_ReturnsSuccessResponse()
        {
            // Arrange
            var newUser = new Usuario { Nombre = "John Doe", DNI = "789-356-2" };

            // Act
            var response = await _usuariosService.Add(newUser);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(newUser, response.Modelo);
        }

        [TestMethod]
        public async Task Add_InvalidUser_ReturnsFailureResponse()
        {
            // Arrange
            var newUser = new Usuario { Nombre = null, DNI = null };

            // Act
            var response = await _usuariosService.Add(newUser);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(newUser, response.Modelo);
        }
        [TestMethod]
        public async Task GetAll_UsersExist()
        {

            // Act
            var response = await _usuariosService.GetAll();

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task GetAll_NoUsersExist()
        {
            // Act
            var response = await _usuariosService.GetAll();

            // Assert
            Assert.IsFalse(!response.IsSuccess);
            Assert.IsNotNull(response.Modelo);
        }

        [TestMethod]
        public async Task Update_ValidUser()
        {
            // Arrange
            var updateUser = new Usuario
            {
                Id = 22,
                Nombre = "Wilma Rabat",
                DNI = "396-83-3666"
            };
            // Act
            var response = await _usuariosService.Update(updateUser);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(updateUser, response.Modelo);
        }

        [TestMethod]
        public async Task Update_InvalidUser()
        {
            // Arrange
            var updateUser = new Usuario
            {
                Id = 100,
                Nombre = "John Doe",
                DNI = "123456789"
            };

            // Act
            var response = await _usuariosService.Update(updateUser);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNull(response.Modelo);
        }

    }
}
