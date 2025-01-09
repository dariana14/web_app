using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class SubjectRepository: BaseEntityRepository<App.Domain.Subject, DAL.DTO.Subject, AppDbContext>,
    ISubjectRepository
{
    public SubjectRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<App.Domain.Subject, DAL.DTO.Subject>(mapper))
    {
    }

    public new async Task<IEnumerable<DAL.DTO.Subject>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        var res = await CreateQuery(userId, noTracking)
            .Include(s => s.AppUser)
            .ToListAsync();

        return res.Select(e => Mapper.Map(e));
    }

}