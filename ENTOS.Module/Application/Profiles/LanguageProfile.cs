using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class LanguageProfile : Profile
    {
	    public LanguageProfile()
        {
            CreateMap<Module.BusinessObjects.Language, Application.DTOs.LanguageDto>();
            CreateMap<Application.DTOs.LanguageDto, Module.BusinessObjects.Language>();
        }
	}
}
