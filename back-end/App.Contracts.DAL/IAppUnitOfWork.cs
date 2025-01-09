using App.Contracts.DAL.Repositories;
using App.Domain.Identity;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IGradeRepository Grades { get; }
    ISubjectRepository Subjects { get; }
    IStudentSubjectRepository StudentSubjects { get; }
    IEntityRepository<AppUser> Users { get; }
    Task<bool> IsAlreadyDeclaredInThisSemesterAsync(Guid subjectId, Guid userId);
}