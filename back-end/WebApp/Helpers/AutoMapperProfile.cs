using AutoMapper;

namespace WebApp.Helpers;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.BLL.DTO.Category, App.DTO.v1_0.Category>().ReverseMap();
        CreateMap<App.BLL.DTO.Rating, App.DTO.v1_0.Rating>().ReverseMap();
        CreateMap<App.BLL.DTO.PetCategory, App.DTO.v1_0.PetCategory>().ReverseMap();
        CreateMap<App.BLL.DTO.ServicePetCategory, App.DTO.v1_0.ServicePetCategory>().ReverseMap();
        CreateMap<App.BLL.DTO.Service, App.DTO.v1_0.Service>().ReverseMap();
        CreateMap<App.BLL.DTO.Location, App.DTO.v1_0.Location>().ReverseMap();
        CreateMap<App.BLL.DTO.Price, App.DTO.v1_0.Price>().ReverseMap();
        CreateMap<App.BLL.DTO.Status, App.DTO.v1_0.Status>().ReverseMap();
        CreateMap<App.BLL.DTO.Advertisement, App.DTO.v1_0.Advertisement>().ReverseMap();
    }
}