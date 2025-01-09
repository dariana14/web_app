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
    
    
    private ISubjectRepository? _subjects;
    public ISubjectRepository Subjects => _subjects ?? new SubjectRepository(UowDbContext, _mapper);
    
    private IStudentSubjectRepository? _studentSubjects;
    public IStudentSubjectRepository StudentSubjects => _studentSubjects ?? new StudentSubjectRepository(UowDbContext, _mapper);
    
    private IGradeRepository? _grades;
    public IGradeRepository Grades => _grades ?? new GradeRepository(UowDbContext, _mapper);

    private IEntityRepository<AppUser>? _users;

    public IEntityRepository<AppUser> Users => _users ??
                                               new BaseEntityRepository<AppUser, AppUser, AppDbContext>(UowDbContext,
                                                   new DalDomainMapper<AppUser, AppUser>(_mapper));
    
    
    public async Task<bool> IsAlreadyDeclaredInThisSemesterAsync(Guid subjectId, Guid userId)
    {
        // Fetch the student's subjects asynchronously
        var studentSubjects = await StudentSubjects.GetAllAsync(userId);
    
        //Fetch the subject details asynchronously
        var subject = await Subjects.FirstOrDefaultAsync(subjectId);
        
        // If the subject does not exist, return false
        if (subject == null)
        {
            return true;
        }
        //Check if any student subject matches the criteria
        return studentSubjects.Any(studentSubject => 
            studentSubject.SubjectId == subjectId);
    }
}