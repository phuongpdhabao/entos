using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class BatchTranslateProfile : Profile
    {
	    public BatchTranslateProfile()
        {
            CreateMap<Module.BusinessObjects.BatchTranslate, Application.DTOs.BatchTranslateDto>();
            CreateMap<Application.DTOs.BatchTranslateDto, Module.BusinessObjects.BatchTranslate>();
        }
	}
}
