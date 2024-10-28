using AutoMapper;

namespace App.BLL;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.Domain.Rating, App.BLL.DTO.Rating>().ReverseMap();
        CreateMap<App.Domain.Category, App.BLL.DTO.Category>().ReverseMap();
        CreateMap<App.Domain.PetCategory, App.BLL.DTO.PetCategory>().ReverseMap();
        CreateMap<App.Domain.ServicePetCategory, App.BLL.DTO.ServicePetCategory>()
            .ForMember(bllS => bllS.PetCategoryName,
                options =>
                    options.MapFrom(domS => domS.PetCategory!.PetCategoryName));
        CreateMap<App.BLL.DTO.ServicePetCategory, App.Domain.ServicePetCategory>();
        CreateMap<App.Domain.Service, App.BLL.DTO.Service>().ReverseMap();
        CreateMap<App.Domain.Location, App.BLL.DTO.Location>().ReverseMap();
        CreateMap<App.Domain.Price, App.BLL.DTO.Price>().ReverseMap();
        CreateMap<App.Domain.Status, App.BLL.DTO.Status>().ReverseMap();
        CreateMap<App.Domain.Advertisement, App.BLL.DTO.Advertisement>().
            ForMember(bllA => bllA.City,
                options =>
                    options.MapFrom(domA => domA.Location!.City)).
            ForMember(bllA => bllA.PriceValue,
                options =>
                    options.MapFrom(domA => domA.Price!.Value)).
            ForMember(bllA => bllA.Description,
                options =>
                    options.MapFrom(domA => domA.Service!.Description)).
            ForMember(bllA => bllA.CategoryName,
                options =>
                    options.MapFrom(domA => domA.Service!.Category!.CategoryName)).
            ForMember(bllA => bllA.StatusName,
                options =>
                    options.MapFrom(domA => domA.Status!.StatusName));
        CreateMap<App.BLL.DTO.Advertisement, App.Domain.Advertisement>();
    }
}