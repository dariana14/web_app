using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IGradeService: IEntityRepository<App.BLL.DTO.Grade>, IGradeRepositoryCustom<App.BLL.DTO.Grade>
{
    
}