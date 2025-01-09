using AutoMapper;

namespace App.BLL;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.DAL.DTO.Grade, App.BLL.DTO.Grade>().ReverseMap();
    }
}