using user_details_service.Controllers;
using user_details_service.DTOs;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using application_infrastructure.Entities;
using application_infrastructure.DBContexts;
using application_infrastructure.TokenService;
using application_infrastructure.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace user_details_service.tests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private Mock<UserManager<User>> _mockUserManager;
    private Mock<ApplicationDbContext> _mockDbContext;
    private Mock<ITokenService> _mockTokenService;
    private Mock<ILoggerManager> _mockLogger;
    private AuthController _authController;

    [SetUp]
    public void Setup()
    {
        _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _mockDbContext = new Mock<ApplicationDbContext>();
        _mockTokenService = new Mock<ITokenService>();
        _mockLogger = new Mock<ILoggerManager>();
        _authController = new AuthController(_mockUserManager.Object, _mockDbContext.Object, _mockTokenService.Object, _mockLogger.Object);
    }

    [Test]
    public async Task Register_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var createUserDto = new CreateUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            QidNumber = "12345",
            Email = "johndoe@example.com",
            Username = "johndoe",
            Password = "password"
        };

        _mockUserManager.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authController.Register(createUserDto);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task Register_InvalidRequest_ReturnsBadRequestResult()
    {
        // Arrange
        var createUserDto = new CreateUserDTO();

        // add model error to mock ModelState
        _authController.ModelState.AddModelError("TestError", "TestErrorMessage");

        // Act
        var result = await _authController.Register(createUserDto);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    [Test]
    public async Task Register_CreateUserFails_ReturnsInternalServerError()
    {
        // Arrange
        var createUserDto = new CreateUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            QidNumber = "12345",
            Email = "johndoe@example.com",
            Username = "johndoe",
            Password = "password"
        };


        // Act
        var result = await _authController.Register(createUserDto);

        // Assert
        Assert.IsInstanceOf<StatusCodeResult>(result);
        Assert.AreEqual(StatusCodes.Status500InternalServerError, (result as StatusCodeResult).StatusCode);
    }
}