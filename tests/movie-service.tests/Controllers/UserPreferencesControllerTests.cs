using System;
using application_infrastructure.Entities;
using application_infrastructure.Logging;
using application_infrastructure.PagingAndSorting;
using application_infrastructure.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using movie_service.Controllers;
using movie_service.DTOs;
using TMDbLib.Objects.Movies;

namespace movie_service.tests.Controllers;

[TestFixture]
public class UserPreferencesControllerTests
{
    private Mock<IWatchLaterRepository> _watchLaterRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ILoggerManager> _loggerMock;
    private Mock<IMapper> _mapperMock;

    private UserPreferencesController _controller;

    [SetUp]
    public void Setup()
    {
        _watchLaterRepositoryMock = new Mock<IWatchLaterRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILoggerManager>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WatchLater, CreateWatchLaterDTO>();
        });

        _mapperMock = new Mock<IMapper>();

        _controller = new UserPreferencesController(
            _watchLaterRepositoryMock.Object,
            _userRepositoryMock.Object,
            Mock.Of<IRatingRepository>(),
            Mock.Of<ISearchHistoryRepository>(),
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Test]
    public async Task GetWatchList_ReturnsBadRequest_WhenUserIdIsEmpty()
    {
        // Arrange
        string userId = "";

        // Act
        var result = await _controller.GetWatchList(userId);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.That(badRequestResult.Value, Is.EqualTo("Empty User Id"));
    }

    [Test]
    public async Task GetWatchList_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        string userId = "123";
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.GetWatchList(userId);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = (NotFoundObjectResult)result;
        Assert.That(notFoundResult.Value, Is.EqualTo("User does not exist"));
        _loggerMock.Verify(logger => logger.LogError($"User with id {userId} does not exist"), Times.Once);
    }

    [Test]
    public async Task GetWatchList_ReturnsOk_WhenWatchListExists()
    {
        // Arrange
        string userId = "123";
        var user = new User { Id = userId };
        var watchList = new List<WatchLater> { new WatchLater { UserId = userId, ProgramId = 1 } };
        var expected = new List<CreateWatchLaterDTO> { new CreateWatchLaterDTO { ProgramId = 1 } };

        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _watchLaterRepositoryMock.Setup(repo => repo.GetWatchListByUserId(userId)).Returns(watchList);

        // Act
        var result = await _controller.GetWatchList(userId);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(expected));
    }

    [Test]
    public async Task GetWatchList_ReturnsInternalServerError_WhenExceptionOccurs()
    {
        // Arrange
        string userId = "123";
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userId)).Throws(new Exception("Something went wrong"));

        // Act
        var result = await _controller.GetWatchList(userId);

        // Assert
        Assert.That(result, Is.TypeOf<StatusCodeResult>());
        var statusCodeResult = (StatusCodeResult)result;
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

    }

    [Test]
    public async Task AddToWatchLater_ValidRequest_ReturnsOk()
    {
        // Arrange
        var request = new CreateWatchLaterDTO { UserId = "1", ProgramId = 123 };
        var user = new User { Id = "1" };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(request.UserId))
            .ReturnsAsync(user);
        _mapperMock.Setup(mapper => mapper.Map<WatchLater>(request))
            .Returns(new WatchLater { UserId = user.Id, ProgramId = request.ProgramId });
        _watchLaterRepositoryMock.Setup(repo => repo.AddToWatchLater(It.IsAny<WatchLater>()))
            .Returns(new WatchLater());

        // Act
        var result = await _controller.AddToWatchLater(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        _watchLaterRepositoryMock.Verify(repo => repo.AddToWatchLater(It.IsAny<WatchLater>()), Times.Once);
    }
}