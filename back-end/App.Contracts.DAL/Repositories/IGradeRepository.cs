using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IGradeRepository: IEntityRepository<App.DAL.DTO.Grade>, IGradeRepositoryCustom<App.DAL.DTO.Grade>
{
    
    
}

public interface IGradeRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllByStudentSubjectIdAsync(Guid studentSubjectId);
    
}