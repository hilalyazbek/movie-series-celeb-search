using System;
using application_infrastructure.Entities;
using AutoMapper;
using movie_service.DTOs;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace movie_service.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<MovieDTO, Movie>().ReverseMap();
        CreateMap<TVShowDTO, TvShow>().ReverseMap();
        CreateMap<CelebrityDTO, Person>().ReverseMap();
        CreateMap<CreateWatchLaterDTO, WatchLater>().ReverseMap();
        CreateMap<DeleteWatchLaterDTO, WatchLater>().ReverseMap();
        CreateMap<ViewRatingDTO, Rating>().ReverseMap();
        CreateMap<UpdateRatingDTO, Rating>().ReverseMap();
        CreateMap<SearchHistoryDTO, SearchHistory>().ReverseMap();
    }
}

