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
    public static List<Movie> SearchMovies(string query, PagingParameters pagingParameters, SortingParameters sortingParameters)
    {
        var result = new List<Movie>();
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchResults = client.SearchMovieAsync(query).Result.Results.Skip(pagingParameters.PageSize * pagingParameters.PageNumber).Take(pagingParameters.PageSize);

        foreach (var movie in searchResults)
        {
            
            result.Add(client.GetMovieAsync(movie.Id).Result);
        }

        return result;
    }

    public static List<TvShow> SearchTvSeason(string query, PagingParameters pagingParameters, SortingParameters sortingParameters)
    {
        var result = new List<TvShow>();
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchResults = client.SearchTvShowAsync(query).Result.Results.Skip(pagingParameters.PageSize * pagingParameters.PageNumber).Take(pagingParameters.PageSize);

        foreach (var tvShow in searchResults)
        {
            result.Add(client.GetTvShowAsync(tvShow.Id).Result);
        }

        return result;
    }

    public static List<Person> SearchCelebrity(string query, PagingParameters pagingParameters, SortingParameters sortingParameters)
    {
        var result = new List<Person>();
        
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchResults = client.SearchPersonAsync(query).Result.Results.Skip(pagingParameters.PageSize * pagingParameters.PageNumber).Take(pagingParameters.PageSize);

        foreach (var celebrity in searchResults)
        {
            result.Add(client.GetPersonAsync(celebrity.Id).Result);
        }

        return result;
    }
}