using System;
using AutoMapper;
using user_details_service.DTOs;
using user_details_service.Entities;

namespace user_details_service.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserDTO, User>().ReverseMap();
        CreateMap<UpdateUserDTO, User>().ReverseMap();
        CreateMap<ViewUserDTO, User>().ReverseMap(); 
    }
}

