using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IAdvertisementRepository Advertisements { get; }
    
    IRatingRepository Ratings { get; }
    ILocationRepository Locations { get; }
    
    IPriceRepository Prices { get; }
    
    IStatusRepository Statuses { get; }
    
    ICategoryRepository Categories { get; }
    
    IPetCategoryRepository PetCategories { get; }

    IServicePetCategoryRepository ServicePetCategories { get; }
    IServiceRepository Services { get; }

    IEntityRepository<AppUser> Users { get; }
}