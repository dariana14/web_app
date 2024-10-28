using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RatingRepository: BaseEntityRepository<App.Domain.Rating, App.Domain.Rating, AppDbContext>,
    IRatingRepository
{
    public RatingRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<App.Domain.Rating, App.Domain.Rating>(mapper))
    {
    }
    
    public async Task<IEnumerable<Domain.Rating>> GetAllByAdvertisementIdAsync(Guid advertisementId)
    {
        return await RepoDbSet
            .Where(a => a.AdvertisementId == advertisementId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Domain.Rating>> GetAllByAdvertisementAndUserIdAsync(Guid advertisementId, Guid userId)
    {
        return await RepoDbSet
                .Where(a => a.AppUserId == userId)
                .Where(a => a.AdvertisementId == advertisementId)
                .ToListAsync();
    }
    
    public async Task<int> RemoveByAdvertisementIdAsync(Guid advertisementId)
    {
        return await RepoDbSet.Where(e => e.AdvertisementId.Equals(advertisementId)).ExecuteDeleteAsync();
    }
}