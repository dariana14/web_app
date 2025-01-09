using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IStudentSubjectRepository: IEntityRepository<App.DAL.DTO.StudentSubject>
{
    Task<IEnumerable<StudentSubject>> GetAllBySubjectIdAsync(Guid subjectId);
    
}