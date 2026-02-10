using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class CountryProfile : Profile
    {
	    public CountryProfile()
        {
            CreateMap<Module.BusinessObjects.Country, Application.DTOs.CountryDto>();
            CreateMap<Application.DTOs.CountryDto, Module.BusinessObjects.Country>();
        }
	}
}
