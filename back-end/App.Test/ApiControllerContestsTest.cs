using App.BLL;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain.Identity;
using App.DTO.v1_0;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using WebApp.ApiControllers;

namespace App.Test;

public class ApiControllerContestsTest
{
    private AppDbContext _ctx;
    
    private IAppUnitOfWork _uow;

    private UserManager<AppUser> _userManager;

    // sut
    private SubjectsController _controller;

    public ApiControllerContestsTest()
    {
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        _ctx = new AppDbContext(optionsBuilder.Options);

        var configUow =
            new MapperConfiguration(cfg => cfg.CreateMap<App.Domain.Subject, DAL.DTO.Subject>().ReverseMap());
        var mapperUow = configUow.CreateMapper();


        _uow = new AppUOW(_ctx, mapperUow);

        var configWeb = new MapperConfiguration(cfg => cfg.CreateMap<App.DTO.v1_0.UserTeacher, App.Domain.Identity.AppUser>().ReverseMap());
        var mapperWeb = configWeb.CreateMapper();


        var storeStub = Substitute.For<IUserStore<AppUser>>();
        var optionsStub = Substitute.For<IOptions<IdentityOptions>>();
        var hasherStub = Substitute.For<IPasswordHasher<AppUser>>();

        var validatorStub = Substitute.For<IEnumerable<IUserValidator<AppUser>>>();
        var passwordStup = Substitute.For<IEnumerable<IPasswordValidator<AppUser>>>();
        var lookupStub = Substitute.For<ILookupNormalizer>();
        var errorStub = Substitute.For<IdentityErrorDescriber>();
        var serviceStub = Substitute.For<IServiceProvider>();
        var loggerStub = Substitute.For<ILogger<UserManager<AppUser>>>();

        _userManager = Substitute.For<UserManager<AppUser>>(
            storeStub, 
            optionsStub, 
            hasherStub,
            validatorStub, passwordStup, lookupStub, errorStub, serviceStub, loggerStub
        );

        
        _controller = new SubjectsController(_ctx, _uow, _userManager, mapperWeb);
        _userManager.GetUserId(_controller.User).Returns(Guid.NewGuid().ToString());

    }

    [Fact]
    public async Task GetTest()
    {
        var result = await _controller.GetSubjectsWithoutLogin();
        var okRes = result.Result as OkObjectResult;
        var val = okRes!.Value as List<App.DAL.DTO.Subject>;
        // Assert.Empty(val);
        Console.WriteLine(result);
    }
}