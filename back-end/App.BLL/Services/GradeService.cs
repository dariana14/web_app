using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class GradeService :
    BaseEntityService<App.DAL.DTO.Grade, App.BLL.DTO.Grade, IGradeRepository>,
    IGradeService
{
    public GradeService(IAppUnitOfWork uoW, IGradeRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Grade, App.BLL.DTO.Grade>(mapper))
    {
    }

    public async Task<IEnumerable<BLL.DTO.Grade>> GetAllByStudentSubjectIdAsync(Guid studentSubjectId)
    {
        return (await Repository.GetAllByStudentSubjectIdAsync(studentSubjectId)).Select(e => Mapper.Map(e));
    }
}