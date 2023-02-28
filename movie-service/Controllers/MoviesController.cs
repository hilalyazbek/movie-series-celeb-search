using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using movie_service.HttpClients;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace movie_service.Controllers;

[ApiController]
[Route("movies")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MoviesController : ControllerBase
{
    private readonly ILogger<MoviesController> _logger;

    public MoviesController(ILogger<MoviesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{query}")]
    public ActionResult Get(string query)
    {
        List<SearchMovie> movies = TmdbService.SearchMovies(query);
        if(movies is null)
        {
            return NotFound($"No movies with {query} in their title were found");
        }
        return Ok(movies);
    }
}

