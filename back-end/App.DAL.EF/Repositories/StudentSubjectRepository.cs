using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class StudentSubjectRepository: BaseEntityRepository<App.Domain.StudentSubject, DAL.DTO.StudentSubject, AppDbContext>,
    IStudentSubjectRepository
{
    public StudentSubjectRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<App.Domain.StudentSubject, DAL.DTO.StudentSubject>(mapper))
    {
    }
    
    public async Task<IEnumerable<DAL.DTO.StudentSubject>> GetAllBySubjectIdAsync(Guid subjectId)
    {
        var query = CreateQuery(); 
        
        var res = await query
            .Where(s => s.SubjectId == subjectId)
            .Include(s => s.Subject)
            .Include(s => s.AppUser)
            .ToListAsync();
        
        return res.Select(e => Mapper.Map(e));
    }
    
    public new async Task<IEnumerable<DAL.DTO.StudentSubject>> GetAllAsync(Guid userId = default, bool noTracking = true)
    {
        var res = await CreateQuery(userId, noTracking)
            .Include(s => s.Subject)
            .Include(s => s.AppUser)
            .ToListAsync();

        return res.Select(e => Mapper.Map(e));
    }

}