using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using App.Domain.Identity;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }
    
    private ILocationRepository? _locations;
    public ILocationRepository Locations => _locations ?? new LocationRepository(UowDbContext, _mapper);
    
    private IRatingRepository? _ratings;
    public IRatingRepository Ratings => _ratings ?? new RatingRepository(UowDbContext, _mapper);
    
    private IAdvertisementRepository? _advertisements;
    public IAdvertisementRepository Advertisements => _advertisements ?? new AdvertisementRepository(UowDbContext, _mapper);
    private IPriceRepository? _prices { get; }
    public IPriceRepository Prices => _prices ?? new PriceRepository(UowDbContext, _mapper);
    private IStatusRepository? _statuses { get; }
    public IStatusRepository Statuses => _statuses ?? new StatusRepository(UowDbContext, _mapper);
    private ICategoryRepository? _categories { get; }
    
    public ICategoryRepository Categories => _categories ?? new CategoryRepository(UowDbContext, _mapper);
    private IPetCategoryRepository? _petCategories { get; }
    
    public IPetCategoryRepository PetCategories => _petCategories ?? new PetCategoryRepository(UowDbContext, _mapper);
    private IServicePetCategoryRepository? _servicePetCategories { get; }
    
    public IServicePetCategoryRepository ServicePetCategories => _servicePetCategories ?? new ServicePetCategoryRepository(UowDbContext, _mapper);
    private IServiceRepository? _services { get; }
    public IServiceRepository Services => _services ?? new ServiceRepository(UowDbContext, _mapper);

    private IEntityRepository<AppUser>? _users;

    public IEntityRepository<AppUser> Users => _users ??
                                               new BaseEntityRepository<AppUser, AppUser, AppDbContext>(UowDbContext,
                                                   new DalDomainMapper<AppUser, AppUser>(_mapper));
}