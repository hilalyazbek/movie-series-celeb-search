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
[Route("movies")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MoviesController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public MoviesController(ILoggerManager logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult Get([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,
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
}

