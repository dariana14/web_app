using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using App.Domain;
using AutoMapper;
using Base.BLL;
using Base.Contracts.DAL;

namespace App.BLL.Services;

public class RatingService: 
    BaseEntityService<App.Domain.Rating, App.BLL.DTO.Rating, IRatingRepository>, IRatingService
{
public RatingService(IUnitOfWork uoW, IRatingRepository repository, IMapper mapper) 
    : base(uoW, repository, new BllDalMapper<App.Domain.Rating, App.BLL.DTO.Rating>(mapper))
{
}
    public async Task<IEnumerable<App.BLL.DTO.Rating>> GetAllByAdvertisementIdAsync(Guid advertisementId)
    {
        return (await Repository.GetAllByAdvertisementIdAsync(advertisementId)).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<App.BLL.DTO.Rating>> GetAllByAdvertisementAndUserIdAsync(Guid advertisementId, Guid userId)
    {
        return (await Repository.GetAllByAdvertisementAndUserIdAsync(advertisementId, userId)).Select(e => Mapper.Map(e))!;
    }

    public Task<int> RemoveByAdvertisementIdAsync(Guid advertisementId)
    {
        return Repository.RemoveByAdvertisementIdAsync(advertisementId);
    }
}