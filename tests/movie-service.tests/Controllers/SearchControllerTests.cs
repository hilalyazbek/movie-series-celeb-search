using application_infrastructure.Entities;
using application_infrastructure.Logging;
using application_infrastructure.PagingAndSorting;
using application_infrastructure.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using movie_service.Controllers;
using movie_service.DTOs;
using movie_service.HttpClients;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;

namespace movie_service.tests.Controllers;

[TestFixture]
internal class SearchControllerTests
{
    private Mock<ISearchHistoryRepository> _searchHistoryRepositoryMock;
    private Mock<ILoggerManager> _loggerMock;
    private Mock<IMapper> _mapperMock;
    private SearchController _controller;

    [SetUp]
    public void SetUp()
    {
        _searchHistoryRepositoryMock = new Mock<ISearchHistoryRepository>();
        _loggerMock = new Mock<ILoggerManager>();
        _mapperMock = new Mock<IMapper>();
        _controller = new SearchController(
            _searchHistoryRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Test]
    public void SearchMovies_ReturnsOkResult_WhenMoviesFound()
    {
        // Arrange
        var searchQuery = "test";
        var pagingParameters = new PagingParameters();
        var sortingParameters = new SortingParameters();
        var movies = new List<Movie>
        {
            new Movie { Title = "Test Movie" }
        };
        _mapperMock.Setup(m => m.Map<IEnumerable<MovieDTO>>(It.IsAny<IEnumerable<Movie>>()))
            .Returns(new List<MovieDTO> { new MovieDTO { Title = "Test Movie" } });

        // Act
        var result = _controller.SearchMovies(searchQuery, pagingParameters, sortingParameters);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.Value, Is.InstanceOf<IEnumerable<MovieDTO>>());
        var moviesResult = okResult.Value as IEnumerable<MovieDTO>;
        Assert.Multiple(() =>
        {
            Assert.That(moviesResult.Count(), Is.EqualTo(1));
            Assert.That(moviesResult.First().Title, Is.EqualTo("Test Movie"));
        });
        _searchHistoryRepositoryMock.Verify(r => r.Save(It.IsAny<SearchHistory>()), Times.Once);
        _loggerMock.Verify(l => l.LogInfo(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void SearchSeries_ReturnsNotFound_WhenNoSeriesFound()
    {
        // Arrange
        var searchQuery = "non-existent series";
        var pagingParameters = new PagingParameters();
        var sortingParameters = new SortingParameters();

        // Act
        var result = _controller.SearchSeries(searchQuery, pagingParameters, sortingParameters);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult.Value, Is.EqualTo($"No series with {searchQuery} in their title were found"));
    }
}
