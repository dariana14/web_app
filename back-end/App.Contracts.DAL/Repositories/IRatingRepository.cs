using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IRatingRepository: IEntityRepository<App.Domain.Rating>, IRatingRepositoryCustom<App.Domain.Rating>
{
    
}
public interface IRatingRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllByAdvertisementIdAsync(Guid advertisementId);

    Task<IEnumerable<TEntity>> GetAllByAdvertisementAndUserIdAsync(Guid advertisementId, Guid userId);

    Task<int> RemoveByAdvertisementIdAsync(Guid advertisementId);

}