using System;
using application_infrastructure.Entities;
using application_infrastructure.Logging;
using application_infrastructure.PagingAndSorting;
using application_infrastructure.Repositories;
using application_infrastructure.Repositories.Interfaces;
using application_infrastructure.TokenService;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using user_details_service.Controllers;
using user_details_service.DTOs;
namespace user_details_service.tests.Controllers;

[TestFixture]
public class UsersControllerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private TokenService _tokenService;
    private IMapper _mapper;
    private Mock<ILoggerManager> _loggerMock;

    private UsersController _controller;


    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenService = new TokenService();
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfiles.MappingProfile());
        });
        _mapper = mapperConfig.CreateMapper();
        _loggerMock = new Mock<ILoggerManager>();

        _controller = new UsersController(_userRepositoryMock.Object,
                                           _tokenService,
                                           _mapper,
                                           _loggerMock.Object);
    }

    [Test]
    public async Task GetUsers_ReturnsOkResult_WithListOfViewUserDTO()
    {
        // Arrange
        var userParameters = new PagingParameters();
        var sortingParameters = new SortingParameters();
        var users = new List<User>();
        _userRepositoryMock.Setup(repo => repo.GetUsersAsync(userParameters, sortingParameters))
                            .ReturnsAsync(users);
        var expectedUsers = _mapper.Map<IEnumerable<ViewUserDTO>>(users);

        // Act
        var result = await _controller.GetUsers(userParameters, sortingParameters);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
        var actualUsers = okResult.Value as IEnumerable<ViewUserDTO>;
        Assert.IsNotNull(actualUsers);
        Assert.AreEqual(expectedUsers.Count(), actualUsers.Count());
    }

    [Test]
    public async Task GetUsers_ReturnsStatusCode500_WhenExceptionThrown()
    {
        // Arrange
        var userParameters = new PagingParameters();
        var sortingParameters = new SortingParameters();
        _userRepositoryMock.Setup(repo => repo.GetUsersAsync(userParameters, sortingParameters))
                            .Throws(new Exception("Test Exception"));

        // Act
        var result = await _controller.GetUsers(userParameters, sortingParameters);

        // Assert
        var statusCodeResult = result as StatusCodeResult;
        Assert.IsNotNull(statusCodeResult);
        Assert.AreEqual(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
    }
}