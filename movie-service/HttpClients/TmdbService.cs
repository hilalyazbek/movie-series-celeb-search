using System;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace movie_service.HttpClients;

public static class TmdbService
{
    public static List<SearchMovie> SearchMovies(string query)
    {
        List<SearchMovie> result = new();
        TMDbClient client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        SearchContainer<SearchMovie> searchContainer = client.SearchMovieAsync(query).Result;

        foreach(SearchMovie movie in searchContainer.Results)
        {
            result.Add(movie);
        }

        return result;
    }

    public static List<SearchTv> SearchTvSeason(string query)
    {
        List<SearchTv> result = new();
        TMDbClient client = new TMDbClient("b4deb664afe3d5005f9f04f34dbb32fa");
        SearchContainer<SearchTv> searchContainer = client.SearchTvShowAsync(query).Result;

        foreach (SearchTv series in searchContainer.Results)
        {
            result.Add(series);
        }

        return result;
    }
}