using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Slottet.Application.BusinessLogic;
using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Slottet.Test
{
    [TestClass]
    public class RoleServiceTest
    {
        private Mock<IRoleRepository> _roleRepoMock = null!;
        private Mock<IUnitOfWork> _uowMock = null!;
        private RoleService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _roleRepoMock = new Mock<IRoleRepository>();
            _uowMock = new Mock<IUnitOfWork>();
            
            // Passing the mocked dependencies instead of the real ones, so it's Mock and not real data
            _service = new RoleService(_roleRepoMock.Object, _uowMock.Object);
        }

        [TestMethod]
        public async Task GetAllRoles_ReturnDtos_WhenRolesExist()
        {
            // Arrange - First we make the list, assign the ID's and setup the mock to return it when the service calls the repository
            var roles = new List<Role>
            {
                Role.Create("Admin"),
                Role.Create("User")
            };
            roles[0].Id = 1;
            roles[1].Id = 2;
            
            // Setup the mock repository to return the list of roles when GetAll is called, instead of calling the real database
            _roleRepoMock.Setup(repo => repo.GetAll()).ReturnsAsync(roles);

            // Act - Then we call the service method we want to test
            var result = await _service.GetAllRoles();

            // Assert - Check if everything went as expected
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Admin", result[0].RoleName);
            Assert.AreEqual("User", result[1].RoleName);
        }

        [TestMethod]
        public async Task GetAllRoles_ThrowException_WhenNoRolesExist()
        {
            // Arrange
            _roleRepoMock.Setup(repo => repo.GetAll()).ReturnsAsync(new List<Role>());

            // Act & Assert
            try
            {
                await _service.GetAllRoles();
                Assert.Fail("Expected KeyNotFoundException was not thrown.");
            }
            catch (KeyNotFoundException ex)
            {
                Assert.AreEqual("Ingen roller fundet", ex.Message);
            }
        }

        [TestMethod]
        public async Task Add_ReturnDto_WhenCreated()
        {
            // Act
            var input = new RoleDTO
            {
                RoleName = "Admin"
            };

            _roleRepoMock.Setup(repo => repo.Add(It.IsAny<Role>())).Returns(Task.CompletedTask)
                .Callback<Role>(r => r.Id = 1); // Mock the repository setting the ID after adding

            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var createdRole = Role.Create("Admin");
            createdRole.Id = 1;

            _roleRepoMock.Setup(r => r.GetById(It.IsAny<int>()))
                .ReturnsAsync(createdRole); // Mock fetching the created role

            // Act

            var result = await _service.Add(input);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Admin", result.RoleName);


        }
    }
}