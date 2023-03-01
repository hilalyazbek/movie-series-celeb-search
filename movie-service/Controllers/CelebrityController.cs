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
[Route("celebrity")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CelebrityController : ControllerBase
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CelebrityController(ILoggerManager logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult GetCelebrity([FromQuery] string searchQuery, [FromQuery] PagingParameters pagingParameters,
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

