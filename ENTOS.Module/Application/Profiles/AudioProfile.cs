using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class AudioProfile : Profile
    {
	    public AudioProfile()
        {
            CreateMap<Module.BusinessObjects.Audio, Application.DTOs.AudioDto>();
            CreateMap<Application.DTOs.AudioDto, Module.BusinessObjects.Audio>();
        }
	}
}
