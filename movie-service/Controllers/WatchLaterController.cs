using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using movie_service.HttpClients;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using AutoMapper;
using movie_service.DTOs;
using application_infrastructure.PagingAndSorting;
using application_infrastructure.Repositories.Interfaces;
using application_infrastructure.Logging;
using application_infrastructure.Entities;

namespace movie_service.Controllers;

[ApiController]
[Route("watchlater")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class WatchLaterController : ControllerBase
{
    private readonly IWatchLaterRepository _watchLaterRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public WatchLaterController(IWatchLaterRepository watchLaterRepository, IUserRepository userRepository, ILoggerManager logger, IMapper mapper)
    {
        _watchLaterRepository = watchLaterRepository;
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("/")]
    public async Task<ActionResult<WatchLaterDTO>> AddToWatchLater([FromBody] WatchLaterDTO request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if(user is null)
            {
                _logger.LogError($"user with id {request.UserId} does not exist.");
                return NotFound("User not found");
            }

            var requestToBeCreated = _mapper.Map<WatchLater>(request);

            var result = _watchLaterRepository.AddToWatchLater(requestToBeCreated);

            if(result is null)
            {
                _logger.LogError($"cannot add program with id {request.ProgramId} to the user {request.UserId} watch list");
                return BadRequest("Something went wrong");
            }

            return Ok(request);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

