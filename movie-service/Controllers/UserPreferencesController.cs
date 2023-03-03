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
[Route("userpref")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserPreferencesController : ControllerBase
{
    private readonly IWatchLaterRepository _watchLaterRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UserPreferencesController(
        IWatchLaterRepository watchLaterRepository,
        IUserRepository userRepository,
        IRatingRepository ratingRepository,
        ILoggerManager logger,
        IMapper mapper)
    {
        _watchLaterRepository = watchLaterRepository;
        _userRepository = userRepository;
        _ratingRepository = ratingRepository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("/watchlater/{userId}")]
    public async Task<IActionResult> GetWatchList(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Empty User Id");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);
            if(user is null)
            {
                _logger.LogError($"User with id {userId} does not exist");
                return NotFound("User does not exist");
            }

            var watchList = _watchLaterRepository.GetWatchListByUserId(userId);

            var result = _mapper.Map<IEnumerable<WatchLaterDTO>>(watchList);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("/watchlater/")]
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

    [HttpPost("/ratings/")]
    public async Task<ActionResult<ViewRatingDTO>> RateProgram([FromBody] UpdateRatingDTO request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var requestToBeCreated = _mapper.Map<Rating>(request);

            var updatedRating = _ratingRepository.RateMovie(requestToBeCreated);
            if (updatedRating is null)
            {
                _logger.LogError($"cannot rate program with id {request.ProgramId}");
                return BadRequest("Something went wrong");
            }
            var result = _mapper.Map<ViewRatingDTO>(updatedRating);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("/ratings/")]
    public ActionResult<IEnumerable<ViewRatingDTO>> GetRatings()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var ratings = _ratingRepository.GetRatings();
            if (ratings is null)
            {
                _logger.LogError($"This program is not rated yet");
                return BadRequest("Something went wrong");
            }

            var result = _mapper.Map<IEnumerable<ViewRatingDTO>>(ratings);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("/ratings/{programId}")]
    public ActionResult<ViewRatingDTO> GetRatings(int programId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var rating = _ratingRepository.GetRatingByProgramID(programId);
            if (rating is null)
            {
                _logger.LogError($"This program is not rated yet");
                return BadRequest("Something went wrong");
            }

            var result = _mapper.Map<ViewRatingDTO>(rating);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

