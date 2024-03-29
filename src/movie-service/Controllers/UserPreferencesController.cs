﻿using Microsoft.AspNetCore.Authorization;
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
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserPreferencesController : ControllerBase
{
    private readonly IWatchLaterRepository _watchLaterRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRatingRepository _ratingRepository;
    private readonly ISearchHistoryRepository _searchHistoryRepository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public UserPreferencesController(
        IWatchLaterRepository watchLaterRepository,
        IUserRepository userRepository,
        IRatingRepository ratingRepository,
        ISearchHistoryRepository searchHistoryRepository,
        ILoggerManager logger,
        IMapper mapper)
    {
        _watchLaterRepository = watchLaterRepository;
        _userRepository = userRepository;
        _ratingRepository = ratingRepository;
        _searchHistoryRepository = searchHistoryRepository;
        _logger = logger;
        _mapper = mapper;
    }


    [HttpGet("/watchlater/{userId}")]
    /// <summary>
    /// It gets a user's watch list by their user id
    /// </summary>
    /// <param name="userId">The userId of the user who's watch list we want to retrieve</param>
    /// <returns>
    /// A list of videos that the user has added to their watch later list.
    /// </returns>
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

            var watchList = await _watchLaterRepository.GetWatchListByUserIdAsync(userId);

            var result = _mapper.Map<IEnumerable<CreateWatchLaterDTO>>(watchList);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// It takes a request from the client, checks if the user exists, if the user exists, it adds the
    /// program to the user's watch later list
    /// </summary>
    /// <param name="CreateWatchLaterDTO"></param>
    /// <returns>
    /// The method returns a Task<ActionResult<CreateWatchLaterDTO>>.
    /// </returns>
    [HttpPost("/watchlater/")]
    public async Task<ActionResult<CreateWatchLaterDTO>> AddToWatchLater([FromBody] CreateWatchLaterDTO request)
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
            requestToBeCreated.UserId = user.Id;

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
            return BadRequest(StatusCodes.Status500InternalServerError);
        }
    }

    
    /// <summary>
    /// It deletes a program from a user's watch later list
    /// </summary>
    /// <param name="DeleteWatchLaterDTO"></param>
    /// <returns>
    /// The method returns a boolean value.
    /// </returns>
    [HttpDelete("/watchlater/")]
    public async Task<ActionResult<CreateWatchLaterDTO>> DeleteFromWatchLater([FromBody] DeleteWatchLaterDTO request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user is null)
            {
                _logger.LogError($"user with id {request.UserId} does not exist.");
                return NotFound("User not found");
            }

            var userHasWatchList = await _watchLaterRepository.UserHasWatchListAsync(request.UserId);
            if (!userHasWatchList)
            {
                _logger.LogError($"user {request.UserId} does not have a watch list");
                return NotFound("User does not have a watch list");
            }

            var itemToBeDeleted = await _watchLaterRepository.FindItemInWatchListAsync(request.UserId, request.ProgramId);
            if(itemToBeDeleted is null)
            {
                _logger.LogError($"program with id {request.ProgramId} does not exist in user {request.UserId} watch list");
                return NotFound("Program is not in watch list");
            }

            var deleted = _watchLaterRepository.DeleteFromWatchLater(itemToBeDeleted);
            if (!deleted)
            {
                _logger.LogError($"cannot remove program with id {request.ProgramId} from the user {request.UserId} watch list");
                return BadRequest("Something went wrong");
            }

            return Ok(deleted);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// It takes a request body, maps it to a Rating object, and then calls the RateMovie function in
    /// the repository
    /// </summary>
    /// <param name="UpdateRatingDTO"></param>
    /// <returns>
    /// The rating of the program
    /// </returns>
    [HttpPost("/ratings/")]
    public ActionResult<ViewRatingDTO> RateProgram([FromBody] UpdateRatingDTO request)
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

    /// <summary>
    /// It gets all the ratings from the database and returns them as a list of ViewRatingDTO objects
    /// </summary>
    /// <returns>
    /// A list of ratings
    /// </returns>
    [HttpGet("/ratings/")]
    public async Task<ActionResult<IEnumerable<ViewRatingDTO>>> GetRatings()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var ratings = await _ratingRepository.GetRatingsAsync();
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

    /// <summary>
    /// It gets the rating of a program by its programId
    /// </summary>
    /// <param name="programId">The id of the program that is being rated</param>
    /// <returns>
    /// A ViewRatingDTO object
    /// </returns>
    [HttpGet("/ratings/{programId}")]
    public async  Task<ActionResult<ViewRatingDTO>> GetRatings(int programId)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var rating = await _ratingRepository.GetRatingByProgramIDAsync(programId);
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

    /// <summary>
    /// It returns a list of search history objects from the database
    /// </summary>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of SearchHistoryDTO objects
    /// </returns>
    [HttpGet("/searchhistory")]
    public async Task<ActionResult<SearchHistoryDTO>> GetSearchHistory([FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state");
                return BadRequest(ModelState);
            }

            var searchHistory = await _searchHistoryRepository.GetAllAsync(pagingParameters, sortingParameters);
            if (searchHistory is null)
            {
                _logger.LogError($"No search History");
                return NotFound("No Search History");
            }

            var result = _mapper.Map<List<SearchHistoryDTO>>(searchHistory);

            _logger.LogError($"Search history returned {searchHistory.Count()}");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

