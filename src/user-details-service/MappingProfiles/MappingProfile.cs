using System;
using application_infrastructure.Entities;
using AutoMapper;
using user_details_service.DTOs;

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

