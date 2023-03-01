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

namespace movie_service.Controllers;

[ApiController]
[Route("search")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SearchController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public SearchController(ILoggerManager logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("/movies")]
    public ActionResult SearchMovies([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
    {
        var movies = TmdbService.SearchMovies(searchQuery, pagingParameters, sortingParameters);

        if (movies is null)
        {
            return NotFound($"No movies with {searchQuery} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<MovieDTO>>(movies);
        
        return Ok(result);
    }

    [HttpGet("/series")]
    public ActionResult SearchSeries([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
    {
        var series = TmdbService.SearchTvSeason(searchQuery, pagingParameters, sortingParameters);

        if (series is null)
        {
            return NotFound($"No series with {searchQuery} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<TVShowDTO>>(series);

        return Ok(result);
    }

    [HttpGet("/celebrities")]
    public ActionResult SearchCelebrities([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,
        [FromQuery] SortingParameters sortingParameters)
    {
        var celebrities = TmdbService.SearchCelebrity(searchQuery, pagingParameters, sortingParameters);
        if (celebrities is null)
        {
            return NotFound($"No celebrity with {searchQuery} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<CelebrityDTO>>(celebrities);

        return Ok(result);
    }
}

