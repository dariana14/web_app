using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.DAL.EF;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL: BaseBLL<AppDbContext>, IAppBLL
{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    
    public AppBLL(IAppUnitOfWork uoW, IMapper mapper) : base(uoW)
    {
        _mapper = mapper;
        _uow = uoW;
    }
    
    private ILocationService? _locations;
    public ILocationService Locations => _locations ?? new LocationService(_uow, _uow.Locations, _mapper);
    
    private IRatingService? _ratings;
    public IRatingService Ratings => _ratings ?? new RatingService(_uow, _uow.Ratings, _mapper);
    
    private IAdvertisementService? _advertisements;
    public IAdvertisementService Advertisements => _advertisements ?? new AdvertisementService(_uow, _uow.Advertisements, _mapper);

    private IPriceService? _prices;
    public IPriceService Prices => _prices ?? new PriceService(_uow, _uow.Prices, _mapper);
    
    private IStatusService? _statuses;
    public IStatusService Statuses => _statuses ?? new StatusService(_uow, _uow.Statuses, _mapper);

    private ICategoryService? _categories;
    public ICategoryService Categories => _categories ?? new CategoryService(_uow, _uow.Categories, _mapper);
    
    private IPetCategoryService? _petCategories;
    public IPetCategoryService PetCategories => _petCategories ?? new PetCategoryService(_uow, _uow.PetCategories, _mapper);
    
    private IServicePetCategoryService? _servicePetCategories;
    public IServicePetCategoryService ServicePetCategories => _servicePetCategories ?? new ServicePetCategoryService(_uow, _uow.ServicePetCategories, _mapper);

    private IServiceService? _services;
    public IServiceService Services => _services ?? new ServiceService(_uow, _uow.Services, _mapper);
    
}