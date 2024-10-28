using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IAdvertisementService Advertisements { get; }
    
    IRatingService Ratings { get; }
    
    ILocationService Locations { get; }
    
    IPriceService Prices { get; }
    
    IStatusService Statuses { get; }
    
    ICategoryService Categories { get; }

    IPetCategoryService PetCategories { get; }
    
    IServicePetCategoryService ServicePetCategories { get; }
    IServiceService Services { get; }
}