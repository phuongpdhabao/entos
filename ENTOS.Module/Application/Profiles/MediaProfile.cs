using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class MediaProfile : Profile
    {
	    public MediaProfile()
        {
            CreateMap<Module.BusinessObjects.Media, Application.DTOs.MediaDto>();
            CreateMap<Application.DTOs.MediaDto, Module.BusinessObjects.Media>();
        }
	}
}
