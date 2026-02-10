using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class SpaceProfile : Profile
    {
	    public SpaceProfile()
        {
            CreateMap<Module.BusinessObjects.Space, Application.DTOs.SpaceDto>();
            CreateMap<Application.DTOs.SpaceDto, Module.BusinessObjects.Space>();
        }
	}
}
