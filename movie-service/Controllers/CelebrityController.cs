using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using movie_service.HttpClients;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace movie_service.Controllers;

[ApiController]
[Route("celebrity")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CelebrityController : ControllerBase
{
    private readonly ILogger<CelebrityController> _logger;

    public CelebrityController(ILogger<CelebrityController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{query}")]
    public ActionResult GetCelebrity(string query)
    {
        List<SearchPerson> celebrities = TmdbService.SearchCelebrity(query);
        if (celebrities is null)
        {
            return NotFound($"No celebrity with {query} in their title were found");
        }
        return Ok(celebrities);
    }
}

