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
    public static List<Movie> SearchMovies(string query)
    {
        var result = new List<Movie>();
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchContainer = client.SearchMovieAsync(query).Result;

        foreach(var movie in searchContainer.Results)
        {
            
            result.Add(client.GetMovieAsync(movie.Id).Result);
        }

        return result;
    }

    public static List<TvShow> SearchTvSeason(string query)
    {
        var result = new List<TvShow>();
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchContainer = client.SearchTvShowAsync(query).Result;

        foreach (var tvShow in searchContainer.Results)
        {
            result.Add(client.GetTvShowAsync(tvShow.Id).Result);
        }

        return result;
    }

    public static List<Person> SearchCelebrity(string query)
    {
        var result = new List<Person>();
        
        var client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        var searchContainer = client.SearchPersonAsync(query).Result;

        foreach (var celebrity in searchContainer.Results)
        {
            result.Add(client.GetPersonAsync(celebrity.Id).Result);
        }

        return result;
    }
}