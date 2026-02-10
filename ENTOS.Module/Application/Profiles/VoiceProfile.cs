using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class VoiceProfile : Profile
    {
	    public VoiceProfile()
        {
            CreateMap<Module.BusinessObjects.Voice, Application.DTOs.VoiceDto>();
            CreateMap<Application.DTOs.VoiceDto, Module.BusinessObjects.Voice>();
        }
	}
}
