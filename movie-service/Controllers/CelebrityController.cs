using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using movie_service.HttpClients;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using AutoMapper;
using movie_service.DTOs;

namespace movie_service.Controllers;

[ApiController]
[Route("celebrity")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CelebrityController : ControllerBase
{
    private readonly ILogger<CelebrityController> _logger;
    private readonly IMapper _mapper;

    public CelebrityController(ILogger<CelebrityController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("{query}")]
    public ActionResult GetCelebrity(string query)
    {
        var celebrities = TmdbService.SearchCelebrity(query);
        if (celebrities is null)
        {
            return NotFound($"No celebrity with {query} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<CelebrityDTO>>(celebrities);

        return Ok(result);
    }
}

