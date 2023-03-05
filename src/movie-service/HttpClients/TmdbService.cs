using application_infrastructure.PagingAndSorting;
using System;
using System.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace movie_service.HttpClients;

public static class TmdbService
{

    /// <summary>
    /// It takes a query, a page number, and a sorting parameter, and returns a list of movies
    /// </summary>
    /// <param name="query">The search query</param>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of movies that match the search query.
    /// </returns>
    public static List<Movie> SearchMovies(string query, PagingParameters pagingParameters, SortingParameters sortingParameters)
    {
        var result = new List<Movie>();
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchResults = client.SearchMovieAsync(query, pagingParameters.PageNumber).Result.Results;
        
        foreach (var movie in searchResults)
        {            
            result.Add(client.GetMovieAsync(movie.Id).Result);
        }

        return result;
    }

    /// <summary>
    /// It searches for a TV show, then gets the details of each TV show that was found
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of TvShow objects.
    /// </returns>
    public static List<TvShow> SearchTvSeason(string query, PagingParameters pagingParameters, SortingParameters sortingParameters)
    {
        var result = new List<TvShow>();
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchResults = client.SearchTvShowAsync(query, pagingParameters.PageNumber).Result.Results;

        foreach (var tvShow in searchResults)
        {
            result.Add(client.GetTvShowAsync(tvShow.Id).Result);
        }

        return result;
    }

    /// <summary>
    /// It takes a query, a page number, and a sorting parameter, and returns a list of people
    /// </summary>
    /// <param name="query">The search query.</param>
    /// <param name="PagingParameters"></param>
    /// <param name="SortingParameters"></param>
    /// <returns>
    /// A list of Person objects.
    /// </returns>
    public static List<Person> SearchCelebrity(string query, PagingParameters pagingParameters, SortingParameters sortingParameters)
    {
        var result = new List<Person>();
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchResults = client.SearchPersonAsync(query, pagingParameters.PageNumber).Result.Results;

        foreach (var celebrity in searchResults)
        {
            result.Add(client.GetPersonAsync(celebrity.Id).Result);
        }

        return result;
    }
}