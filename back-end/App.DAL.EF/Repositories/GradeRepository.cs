using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class GradeRepository: BaseEntityRepository<App.Domain.Grade, DAL.DTO.Grade, AppDbContext>,
    IGradeRepository
{
    public GradeRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<App.Domain.Grade, DAL.DTO.Grade>(mapper))
    {
    }

    public async Task<IEnumerable<DAL.DTO.Grade>> GetAllByStudentSubjectIdAsync(Guid studentSubjectId)
    {
        var res = await RepoDbSet
            .Where(g => g.StudentSubjectId == studentSubjectId)
            .ToListAsync();
        
        return res.Select(e => Mapper.Map(e));
    }
    
}