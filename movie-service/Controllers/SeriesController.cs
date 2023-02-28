using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using movie_service.HttpClients;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace movie_service.Controllers;

[ApiController]
[Route("series")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SeriesController : ControllerBase
{
    private readonly ILogger<SeriesController> _logger;

    public SeriesController(ILogger<SeriesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{query}")]
    public ActionResult Get(string query)
    {
        List<SearchTv> movies = TmdbService.SearchTvSeason(query);
        if(movies is null)
        {
            return NotFound($"No series with {query} in their title were found");
        }
        return Ok(movies);
    }
}