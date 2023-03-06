using System;
using application_infrastructure.Entities;
/* A namespace. */
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
    private Mock<ISearchHistoryRepository> _searchHistoryRepositoryMock;
    private Mock<IWatchLaterRepository> _watchLaterRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IRatingRepository> _ratingRepositoryMock;
    private Mock<ILoggerManager> _loggerMock;
    private Mock<IMapper> _mapperMock;

    private UserPreferencesController _controller;

    [SetUp]
    public void Setup()
    {
        _searchHistoryRepositoryMock = new Mock<ISearchHistoryRepository>();
        _watchLaterRepositoryMock = new Mock<IWatchLaterRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _ratingRepositoryMock = new Mock<IRatingRepository>();
        _loggerMock = new Mock<ILoggerManager>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WatchLater, CreateWatchLaterDTO>();
        });

        _mapperMock = new Mock<IMapper>();

        _controller = new UserPreferencesController(
            _watchLaterRepositoryMock.Object,
            _userRepositoryMock.Object,
            _ratingRepositoryMock.Object,
            _searchHistoryRepositoryMock.Object,
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
        Assert.That(result, Is.InstanceOf<ActionResult<CreateWatchLaterDTO>>());
        _watchLaterRepositoryMock.Verify(repo => repo.AddToWatchLater(It.IsAny<WatchLater>()), Times.Once);
    }

    [Test]
    public async Task AddToWatchLater_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateWatchLaterDTO { UserId = "1", ProgramId = 2 };
        _controller.ModelState.AddModelError("ProgramId", "ProgramId is required");

        // Act
        var result = await _controller.AddToWatchLater(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task AddToWatchLater_WithInvalidUser_ReturnsNotFound()
    {
        // Arrange
        var request = new CreateWatchLaterDTO { UserId = "1", ProgramId = 2 };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(request.UserId)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.AddToWatchLater(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.That(notFoundResult.Value, Is.EqualTo("User not found"));
    }

    [Test]
    public async Task AddToWatchLater_WithInvalidResult_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateWatchLaterDTO { UserId = "1", ProgramId = 2 };
        var user = new User { Id = request.UserId };
        _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(request.UserId)).ReturnsAsync(user);
        _watchLaterRepositoryMock.Setup(repo => repo.AddToWatchLater(It.IsAny<WatchLater>())).Returns((WatchLater)null);

        // Act
        var result = await _controller.AddToWatchLater(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo(500));
    }

    [Test]
    public async Task DeleteFromWatchLater_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var invalidRequest = new DeleteWatchLaterDTO();
        _controller.ModelState.AddModelError("UserId", "UserId is required");

        // Act
        var result = await _controller.DeleteFromWatchLater(invalidRequest);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        _watchLaterRepositoryMock.Verify(r => r.DeleteFromWatchLater(It.IsAny<WatchLater>()), Times.Never);
    }

    [Test]
    public async Task DeleteFromWatchLater_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var request = new DeleteWatchLaterDTO { UserId = "1", ProgramId = 2 };
        _userRepositoryMock.Setup(r => r.GetUserByIdAsync(request.UserId)).ReturnsAsync((User)null);

        // Act
        var result = await _controller.DeleteFromWatchLater(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        _watchLaterRepositoryMock.Verify(r => r.DeleteFromWatchLater(It.IsAny<WatchLater>()), Times.Never);
    }

    [Test]
    public async Task DeleteFromWatchLater_WhenUserDoesNotHaveWatchList_ReturnsNotFound()
    {
        // Arrange
        var request = new DeleteWatchLaterDTO { UserId = "1", ProgramId = 2 };
        _userRepositoryMock.Setup(r => r.GetUserByIdAsync(request.UserId)).ReturnsAsync(new User());
        _watchLaterRepositoryMock.Setup(r => r.UserHasWatchListAsync(request.UserId)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteFromWatchLater(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        _watchLaterRepositoryMock.Verify(r => r.DeleteFromWatchLater(It.IsAny<WatchLater>()), Times.Never);
    }

    [Test]
    public async Task DeleteFromWatchLater_WhenProgramIsNotInWatchList_ReturnsNotFound()
    {
        // Arrange
        var request = new DeleteWatchLaterDTO { UserId = "1", ProgramId = 2 };
        _userRepositoryMock.Setup(r => r.GetUserByIdAsync(request.UserId)).ReturnsAsync(new User());
        _watchLaterRepositoryMock.Setup(r => r.UserHasWatchListAsync(request.UserId)).ReturnsAsync(true);
        _watchLaterRepositoryMock.Setup(r => r.FindItemInWatchListAsync(request.UserId, request.ProgramId)).ReturnsAsync((WatchLater)null);

        // Act
        var result = await _controller.DeleteFromWatchLater(request);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
        _watchLaterRepositoryMock.Verify(r => r.DeleteFromWatchLater(It.IsAny<WatchLater>()), Times.Never);
    }

    [Test]
    public async Task GetRatings_ReturnsOkResult_WhenRatingsExist()
    {
        // Arrange
        var ratings = new List<Rating> { new Rating { ProgramId = 1, RatingValue = 4 } };
        var expectedViewRatings = new List<ViewRatingDTO> { new ViewRatingDTO { ProgramId = 1, RatingValue = 4 } };
        _ratingRepositoryMock.Setup(repo => repo.GetRatingsAsync()).ReturnsAsync(ratings);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ViewRatingDTO>>(ratings)).Returns(expectedViewRatings);

        // Act
        var result = await _controller.GetRatings();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.Value, Is.EqualTo(expectedViewRatings));
    }

    [Test]
    public async Task GetRatings_ReturnsBadRequestResult_WhenRatingsAreNotFound()
    {
        // Arrange
        _ratingRepositoryMock.Setup(repo => repo.GetRatingsAsync()).ReturnsAsync((IEnumerable<Rating>)null);

        // Act
        var result = await _controller.GetRatings();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Something went wrong"));
    }


    [Test]
    public async Task GetRatings_ReturnsInternalServerErrorResult_WhenExceptionIsThrown()
    {
        // Arrange
        _ratingRepositoryMock.Setup(repo => repo.GetRatingsAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _controller.GetRatings();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<StatusCodeResult>());
        var statusCodeResult = result.Result as StatusCodeResult;
        Assert.That(statusCodeResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    } 
}