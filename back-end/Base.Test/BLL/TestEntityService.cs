using AutoMapper;
using Base.BLL;
using Base.Contracts.DAL;
using Base.Test.DAL;
using Base.Test.Domain;

namespace Base.Test.BLL;

public class TestEntityService: BaseEntityService<TestEntity, TestEntity, IEntityRepository<TestEntity>>
{
    public TestEntityService(IUnitOfWork uoW, IEntityRepository<TestEntity> repository, IMapper mapper) : base(uoW,
        repository, new BaseBllDalMapper<TestEntity, TestEntity>(mapper))
    {
    }
}