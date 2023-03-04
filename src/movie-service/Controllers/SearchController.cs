﻿using Microsoft.AspNetCore.Authorization;
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

        _ = _searchHistoryRepository.Save(searchHistoryToBeAdded);

        return Ok(result);
    }

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

        _ = _searchHistoryRepository.Save(searchHistoryToBeAdded);

        return Ok(result);
    }

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

        _ = _searchHistoryRepository.Save(searchHistoryToBeAdded);

        return Ok(result);
    }
}

