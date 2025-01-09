using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<App.Domain.Subject, App.DAL.DTO.Subject>().
            ForMember(dalA => dalA.TeacherFirstName,
                options =>
                    options.MapFrom(domA => domA.AppUser!.FirstName))
            .ForMember(dalA => dalA.TeacherLastName,
                options =>
                    options.MapFrom(domA => domA.AppUser!.LastName))
            .ForMember(dalA => dalA.TeacherEmail,
            options =>
                options.MapFrom(domA => domA.AppUser!.Email));
        CreateMap<App.DAL.DTO.Subject, App.Domain.Subject>();
        CreateMap<App.Domain.StudentSubject, App.DAL.DTO.StudentSubject>()
            .ForMember(dalA => dalA.SubjectName,
            options =>
                options.MapFrom(domA => domA.Subject!.SubjectName))
            .ForMember(dalA => dalA.StudentFullName,
            options =>
                options.MapFrom(domA => domA.AppUser!.FirstName + " " + domA.AppUser!.LastName));
        CreateMap<App.DAL.DTO.StudentSubject, App.Domain.StudentSubject>();
        CreateMap<App.Domain.Grade, App.DAL.DTO.Grade>().ReverseMap();
    }
}