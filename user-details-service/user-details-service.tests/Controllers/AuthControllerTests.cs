using user_details_service.Controllers;
using user_details_service.DTOs;
using user_details_service.Entities;
using user_details_service.Helpers.Logging;
using user_details_service.Infrastructure.DBContexts;
using user_details_service.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace user_details_service.tests.Controllers;

[TestFixture]
public class AuthControllerTests
{
    private Mock<UserManager<User>> _mockUserManager;
    private ApplicationDbContext _context;
    private TokenService _tokenService;
    private ILoggerManager _logger;
    private AuthController _authController;

    [SetUp]
    public void Setup()
    {
        // setup db context
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new ApplicationDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // setup mock user manager
        _mockUserManager = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        _tokenService = new TokenService();
        _logger = new Mock<ILoggerManager>().Object;
        _authController = new AuthController(_mockUserManager.Object, _context, _tokenService, _logger);
    }

    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted();
    }

    [Test]
    public async Task Test_RegisterMethod_WithValidModel_ReturnsCreated()
    {
        // Arrange
        var userDTO = new CreateUserDTO
        {
            FirstName = "wonder",
            LastName = "woman",
            QidNumber = "321321",
            Email = "wonder@woman.com",
            Username = "ww",
            Password = "password"
        };

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authController.Register(userDTO);

        // Assert
        Assert.IsInstanceOf(typeof(CreatedAtActionResult), result);
        var createdResult = (CreatedAtActionResult)result;
        Assert.AreEqual("Register", createdResult.ActionName);
        Assert.AreEqual("wonder@woman.com", createdResult.RouteValues["email"]);
    }

    [Test]
    public async Task Test_Register_WithInvalidModel_ReturnsBadRequestResult()
    {
        // Arrange
        var createUserDTO = new CreateUserDTO();

        // Add model error
        _authController.ModelState.AddModelError("FirstName", "The FirstName field is required.");

        // Act
        var result = await _authController.Register(createUserDTO);

        // Assert
        // Assert.IsFalse);
    }


}