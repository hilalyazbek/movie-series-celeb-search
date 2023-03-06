using System;
using application_infrastructure.Entities;
using application_infrastructure.Logging;
using application_infrastructure.PagingAndSorting;
using application_infrastructure.Repositories;
using application_infrastructure.Repositories.Interfaces;
using application_infrastructure.TokenService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using user_details_service.Controllers;
using user_details_service.DTOs;
namespace user_details_service.tests.Controllers;

[TestFixture]
public class UsersControllerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ITokenService> _tokenServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILoggerManager> _loggerMock;
    private UsersController _controller;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILoggerManager>();

        _controller = new UsersController(
            _userRepositoryMock.Object,
            _tokenServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Test]
    public async Task GetUsers_Returns_OkResult_With_ViewUserDTOs()
    {
        // Arrange
        var pagingParameters = new PagingParameters { PageNumber = 1, PageSize = 10 };
        var sortingParameters = new SortingParameters { SortBy = "FirstName"};

        var users = new List<User>
        {
            new User { Id = "1", FirstName = "John", LastName = "Doe", QidNumber = "123456" },
            new User { Id = "2", FirstName = "Jane", LastName = "Doe", QidNumber = "654321" },
            new User { Id = "3", FirstName = "Bob", LastName = "Smith", QidNumber = "987654" },
        };

        var viewUserDTOs = new List<ViewUserDTO>
        {
            new ViewUserDTO { Id = "1", FirstName = "John", LastName = "Doe", QidNumber = "123456" },
            new ViewUserDTO { Id = "2", FirstName = "Jane", LastName = "Doe", QidNumber = "654321" },
            new ViewUserDTO { Id = "3", FirstName = "Bob", LastName = "Smith", QidNumber = "987654" },
        };

        _userRepositoryMock.Setup(repo => repo.GetUsersAsync(pagingParameters, sortingParameters)).ReturnsAsync(users);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ViewUserDTO>>(users)).Returns(viewUserDTOs);

        // Act
        var result = await _controller.GetUsers(pagingParameters, sortingParameters);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (OkObjectResult)result;
        Assert.AreEqual(okResult.Value, viewUserDTOs);
    }

    [Test]
    public async Task UserDetails_Returns_OkResult_With_ViewUserDTO()
    {
        // Arrange
        var userId = "1";
        var user = new User { Id = userId, FirstName = "John", LastName = "Doe", QidNumber = "123456" };
        var viewUserDTO = new ViewUserDTO { Id = userId, FirstName = "John", LastName = "Doe", QidNumber = "123456" };

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<ViewUserDTO>(user)).Returns(viewUserDTO);

        // Act
        var result = await _controller.UserDetails(userId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);

        var okResult = (OkObjectResult)result;
        Assert.AreEqual(okResult.Value, viewUserDTO);
    }

    [Test]
    public async Task DeleteUser_WhenUserExists_ShouldDeleteUserAndReturnOk()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var userToBeDeleted = new User() { Id = id };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(id))
            .ReturnsAsync(userToBeDeleted);

        // Act
        var result = await _controller.DeleteUser(id);

        // Assert
        _userRepositoryMock.Verify(repo => repo.Delete(userToBeDeleted), Times.Once);
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo($"User with id {id} was deleted successfully"));
    }

    [Test]
    public async Task DeleteUser_WhenUserDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(id))
            .ReturnsAsync((User)null);

        // Act
        var result = await _controller.DeleteUser(id);

        // Assert
        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult.Value, Is.EqualTo($"user with id {id} does not exist"));
    }

    [Test]
    public async Task DeleteUser_WhenRepositoryThrowsException_ShouldReturnInternalServerError()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(id))
            .Throws(new Exception("repository exception"));

        // Act
        var result = await _controller.DeleteUser(id);

        // Assert
        _userRepositoryMock.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
        Assert.That(result, Is.TypeOf<StatusCodeResult>());
        var statusCodeResult = result as StatusCodeResult;
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(500));
    }

    [Test]
    public async Task UserDetails_WithExistingUser_ReturnsOkWithViewUserDTO()
    {
        // Arrange
        var existingUser = new User { Id = "1", FirstName = "John", LastName = "Doe", QidNumber = "1234" };
        var viewUserDTO = new ViewUserDTO { Id = "1", FirstName = "John", LastName = "Doe", QidNumber = "1234" };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(existingUser.Id))
            .ReturnsAsync(existingUser);
        _mapperMock.Setup(mapper => mapper.Map<ViewUserDTO>(existingUser))
            .Returns(viewUserDTO);

        // Act
        var result = await _controller.UserDetails(existingUser.Id);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(viewUserDTO));
        _userRepositoryMock.Verify(repo => repo.GetUserByIdAsync(existingUser.Id), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<ViewUserDTO>(existingUser), Times.Once);
        _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task UserDetails_WithNonExistingUser_ReturnsNotFound()
    {
        // Arrange
        var nonExistingUserId = "1";
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(nonExistingUserId))
            .ReturnsAsync((User)null);

        // Act
        var result = await _controller.UserDetails(nonExistingUserId);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = (NotFoundObjectResult)result;
        Assert.That(notFoundResult.Value, Is.EqualTo($"user with id {nonExistingUserId} does not exist"));
        _userRepositoryMock.Verify(repo => repo.GetUserByIdAsync(nonExistingUserId), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<ViewUserDTO>(It.IsAny<User>()), Times.Never);
        _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>()), Times.Never);
    }


    [Test]
    public async Task UpdateUser_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var id = "123";
        var user = new UpdateUserDTO();
        _controller.ModelState.AddModelError("FirstName", "The FirstName field is required.");

        // Act
        var result = await _controller.UpdateUser(id, user);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        _loggerMock.Verify(x => x.LogError("invalid model state"), Times.Once);
    }

    [Test]
    public async Task UpdateUser_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = "123";
        var user = new UpdateUserDTO();
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.UpdateUser(id, user);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        Assert.AreEqual($"Invalid User with id {id}", ((NotFoundObjectResult)result).Value);
        _loggerMock.Verify(x => x.LogInfo($"user with id {id} does not exist"), Times.Once);
    }

    [Test]
    public async Task UpdateUser_WhenUserFoundAndUpdated_ReturnsOk()
    {
        // Arrange
        var id = "123";
        var user = new UpdateUserDTO
        {
            FirstName = "John",
            LastName = "Doe",
            QidNumber = "123456789"
        };
        var userToBeUpdated = new User
        {
            Id = id,
            FirstName = "Jane",
            LastName = "Doe",
            QidNumber = "987654321"
        };
        var updatedUser = new User
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            QidNumber = "123456789"
        };
        _userRepositoryMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync(userToBeUpdated);
        _userRepositoryMock.Setup(x => x.Update(userToBeUpdated)).Returns(updatedUser);

        // Act
        var result = await _controller.UpdateUser(id, user);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual(updatedUser, ((OkObjectResult)result).Value);
    }
}