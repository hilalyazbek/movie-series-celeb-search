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
using application_infrastructure.Logging;
using application_infrastructure.Repositories.Interfaces;
using application_infrastructure.Entities;

namespace movie_service.Controllers;

[ApiController]
[Route("search")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SearchController : ControllerBase
{
    private readonly ISearchHistoryRepository _searchHistoryRepository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public SearchController(ISearchHistoryRepository searchHistoryRepository, ILoggerManager logger, IMapper mapper)
    {
        _searchHistoryRepository = searchHistoryRepository;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// It searches for movies with a given title, and if it finds any, it saves the search query and
    /// the results to the database
    /// </summary>
    /// <param name="searchQuery">The query to search for.</param>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of movies that match the search query.
    /// </returns>
    [HttpGet("/movies")]
    public ActionResult SearchMovies([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
    {
        var movies = TmdbService.SearchMovies(searchQuery, pagingParameters, sortingParameters);

        if (movies is null || movies.Count == 0)
        {
            return NotFound($"No movies with {searchQuery} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<MovieDTO>>(movies);

        var searchHistoryToBeAdded = new SearchHistory()
        {
            Query = searchQuery,
            Results = result.Select(itm => itm.Title).ToList()
        };
        _logger.LogInfo($"the search for {searchQuery} returned {result.Count()} results");

        _ = _searchHistoryRepository.Save(searchHistoryToBeAdded);

        return Ok(result);
    }

    /// <summary>
    /// It searches for a TV show, and if it finds it, it saves the search query and the results to the
    /// database
    /// </summary>
    /// <param name="searchQuery">The query to search for</param>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of TV shows that match the search query.
    /// </returns>
    [HttpGet("/series")]
    public ActionResult SearchSeries([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
    {
        var series = TmdbService.SearchTvSeason(searchQuery, pagingParameters, sortingParameters);

        if (series is null || series.Count == 0)
        {
            return NotFound($"No series with {searchQuery} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<TVShowDTO>>(series);

        var searchHistoryToBeAdded = new SearchHistory()
        {
            Query = searchQuery,
            Results = result.Select(itm => itm.Name).ToList()
        };
        _logger.LogInfo($"the search for {searchQuery} returned {result.Count()} results");

        _ = _searchHistoryRepository.Save(searchHistoryToBeAdded);

        return Ok(result);
    }

    /// <summary>
    /// It searches for celebrities in the TMDB database, and if it finds any, it saves the search query
    /// and the results in the database
    /// </summary>
    /// <param name="searchQuery">The query to search for</param>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of celebrities that match the search query.
    /// </returns>
    [HttpGet("/celebrities")]
    public ActionResult SearchCelebrities([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
    {
        var celebrities = TmdbService.SearchCelebrity(searchQuery, pagingParameters, sortingParameters);
        if (celebrities is null || celebrities.Count == 0)
        {
            return NotFound($"No celebrity with {searchQuery} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<CelebrityDTO>>(celebrities);

        var searchHistoryToBeAdded = new SearchHistory()
        {
            Query = searchQuery,
            Results = result.Select(itm => itm.Name).ToList()
        };

        _logger.LogInfo($"the search for {searchQuery} returned {result.Count()} results");

        _ = _searchHistoryRepository.Save(searchHistoryToBeAdded);

        return Ok(result);
    }
}

