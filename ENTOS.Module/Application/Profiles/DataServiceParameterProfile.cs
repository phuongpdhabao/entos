using AutoMapper;

namespace ENTOS.Application.Profiles 
{

    public class DataServiceParameterProfile : Profile
    {
	    public DataServiceParameterProfile()
        {
            CreateMap<Module.BusinessObjects.DataServiceParameter, Application.DTOs.DataServiceParameterDto>();
            CreateMap<Application.DTOs.DataServiceParameterDto, Module.BusinessObjects.DataServiceParameter>();
        }
	}
}
