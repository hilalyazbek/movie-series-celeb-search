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
[Route("movies")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class MoviesController : ControllerBase
{
    private readonly ILogger<MoviesController> _logger;
    private readonly IMapper _mapper;

    public MoviesController(ILogger<MoviesController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("{query}")]
    public ActionResult Get(string query)
    {
        var movies = TmdbService.SearchMovies(query);

        if (movies is null)
        {
            return NotFound($"No movies with {query} in their title were found");
        }

        var result = _mapper.Map<IEnumerable<MovieDTO>>(movies);
        
        return Ok(result);
    }
}

