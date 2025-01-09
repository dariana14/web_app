using AutoMapper;

namespace WebApp.Helpers;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.DTO.v1_0.UserTeacher, App.Domain.Identity.AppUser>().ReverseMap();
    }
}