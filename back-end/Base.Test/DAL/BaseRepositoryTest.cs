using AutoMapper;
using Base.DAL.EF;
using Base.Test.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.Test.DAL;

public class BaseRepositoryTest
{
    private readonly TestDbContext _ctx;
    private readonly TestEntityRepository _testEntityRepository;

    public BaseRepositoryTest()
    {
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new TestDbContext(optionsBuilder.Options);

        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg => cfg.CreateMap<TestEntity, TestEntity>());
        var mapper = config.CreateMapper();

        _testEntityRepository =
            new TestEntityRepository(
                _ctx,
                new BaseDalDomainMapper<TestEntity, TestEntity>(mapper)
            );
    }


    [Fact]
    public async Task AddTest()
    {
        // arrange
        _testEntityRepository.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges();

        // act
        var data = await _testEntityRepository.GetAllAsync();

        // assert
        Assert.Equal(1, data.Count());
    }
    
    [Fact]
    public async Task UpdateTest()
    {
        // Arrange
        var entity = new TestEntity() { Value = "Foo" };
        _testEntityRepository.Add(entity);
        _ctx.SaveChanges();

        // Act
        entity.Value = "Bar";
        _testEntityRepository.Update(entity);
        _ctx.SaveChanges();
        var updatedEntity = await _testEntityRepository.FirstOrDefaultAsync(entity.Id);

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal("Bar", updatedEntity.Value);
    }

    [Fact]
    public async Task RemoveAsyncTest()
    {
        // Arrange
        var entity = new TestEntity() { Value = "Foo" };
        _testEntityRepository.Add(entity);
        _ctx.SaveChanges();

        // Act
        var result = await _testEntityRepository.RemoveAsync(entity);
        _ctx.SaveChanges();
        var data = await _testEntityRepository.GetAllAsync();

        // Assert
        Assert.Equal(1, result);
        Assert.Empty(data);
    }

    [Fact]
    public async Task GetAllAsyncTest()
    {
        // Arrange
        _testEntityRepository.Add(new TestEntity() { Value = "Foo" });
        _testEntityRepository.Add(new TestEntity() { Value = "Bar" });
        _ctx.SaveChanges();

        // Act
        var data = await _testEntityRepository.GetAllAsync();

        // Assert
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task FirstOrDefaultAsyncTest()
    {
        // Arrange
        var entity = new TestEntity() { Value = "Foo" };
        _testEntityRepository.Add(entity);
        _ctx.SaveChanges();

        // Act
        var result = await _testEntityRepository.FirstOrDefaultAsync(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Foo", result.Value);
    }

    [Fact]
    public async Task ExistsAsyncTest()
    {
        // Arrange
        var entity = new TestEntity() { Value = "Foo" };
        _testEntityRepository.Add(entity);
        _ctx.SaveChanges();

        // Act
        var exists = await _testEntityRepository.ExistsAsync(entity.Id);

        // Assert
        Assert.True(exists);

        // Act with a non-existing ID
        var nonExists = await _testEntityRepository.ExistsAsync(Guid.NewGuid());

        // Assert
        Assert.False(nonExists);
    }
}