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
    private Mock<ITokenService> _mockTokenService;
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

        _mockTokenService = new Mock<ITokenService>();
        _logger = new Mock<ILoggerManager>().Object;
        _authController = new AuthController(_mockUserManager.Object, _context, _mockTokenService.Object, _logger);
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
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
    }


    [Test]
    public async Task Authenticate_WithValidCredentials_ReturnsOkResultWithAuthToken()
    {
        // Arrange
        var authRequestDTO = new AuthRequestDTO { Email = "test@test.com", Password = "password" };
        var user = new User { UserName = "testuser", Email = "test@test.com" };
        var expectedToken = "expected-token";
        _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        _mockUserManager.Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(true);
        _mockTokenService.Setup(x => x.CreateToken(user)).Returns(expectedToken);

        // Act
        var result = await _authController.Authenticate(authRequestDTO);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var authResponseDTO = ((OkObjectResult)result.Result).Value as AuthResponseDTO;
        Assert.IsNotNull(authResponseDTO);
        Assert.AreEqual(authResponseDTO.UserName, user.UserName);
        Assert.AreEqual(authResponseDTO.Email, user.Email);
        Assert.AreEqual(authResponseDTO.Token, expectedToken);
    }

}