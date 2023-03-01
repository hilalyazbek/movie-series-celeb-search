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
[Route("series")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SeriesController : ControllerBase
{
    private readonly ILogger<SeriesController> _logger;
    private readonly IMapper _mapper;

    public SeriesController(ILogger<SeriesController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("{query}")]
    public ActionResult Get(string query)
    {
        var series = TmdbService.SearchTvSeason(query);
        
        if (series is null)
        {
            return NotFound($"No series with {query} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<TVShowDTO>>(series);
       
        return Ok(result);
    }
}