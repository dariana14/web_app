using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Base.Test.Domain;

namespace Base.Test.DAL;

public class TestUow : BaseUnitOfWork<TestDbContext>, IUnitOfWork
{
    private readonly IMapper _mapper;

    public TestUow(TestDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }
    
    public TestEntityRepository Entities => new TestEntityRepository(UowDbContext, new BaseDalDomainMapper<TestEntity, TestEntity>(_mapper));
}