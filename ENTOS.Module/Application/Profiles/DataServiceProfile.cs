using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class DataServiceProfile : Profile
    {
	    public DataServiceProfile()
        {
            CreateMap<Module.BusinessObjects.DataService, Application.DTOs.DataServiceDto>();
            CreateMap<Application.DTOs.DataServiceDto, Module.BusinessObjects.DataService>();
        }
	}
}
