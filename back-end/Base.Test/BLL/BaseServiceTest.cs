using AutoMapper;
using Base.DAL.EF;
using Base.Test.DAL;
using Base.Test.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.Test.BLL;

public class BaseServiceTest
{
    private readonly TestEntityService _testEntityService;
    private readonly TestDbContext _ctx;
    
    public BaseServiceTest()
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
    
        _testEntityService =
            new TestEntityService( 
                new TestUow(_ctx, mapper), 
                new TestEntityRepository(_ctx, new BaseDalDomainMapper<TestEntity, TestEntity>(mapper)),
                mapper
            );
    }
    
    [Fact]
    public async Task AddTest()
    {
        // arrange
        _testEntityService.Add(new TestEntity() {Value = "Foo"});
        _ctx.SaveChanges();

        // act
        var data = await _testEntityService.GetAllAsync();

        // assert
        Assert.Equal(1, data.Count());
    }
    
    [Fact]
    public void UpdateTest()
    {
        // Arrange
        var entity = new TestEntity() { Value = "Foo" };
        _testEntityService.Add(entity);
        _ctx.SaveChanges();

        // Act
        entity.Value = "Bar";
        _testEntityService.Update(entity);
        _ctx.SaveChanges();
        var updatedEntity = _testEntityService.FirstOrDefault(entity.Id);

        // Assert
        Assert.NotNull(updatedEntity);
        Assert.Equal("Bar", updatedEntity.Value);
    }

    [Fact]
    public async Task RemoveByIdTest()
    {
        // Arrange
        var entity = new TestEntity() { Value = "Foo" };
        _testEntityService.Add(entity);
        _ctx.SaveChanges();

        // Act
        var result = _testEntityService.Remove(entity.Id);
        _ctx.SaveChanges();
        var data = await _testEntityService.GetAllAsync();

        // Assert
        Assert.Equal(1, result);
        Assert.Empty(data);
    }
    
    [Fact]
    public async Task RemoveByEntityTest()
    {
        // Arrange
        var entity = new TestEntity() { Value = "Foo" };
        _testEntityService.Add(entity);
        _ctx.SaveChanges();

        // Act
        var result = _testEntityService.Remove(entity);
        _ctx.SaveChanges();
        var data = await _testEntityService.GetAllAsync();

        // Assert
        Assert.Equal(1, result);
        Assert.Empty(data);
    }

    [Fact]
    public async Task GetAllAsyncTest()
    {
        // Arrange
        _testEntityService.Add(new TestEntity() { Value = "Foo" });
        _testEntityService.Add(new TestEntity() { Value = "Bar" });
        _ctx.SaveChanges();

        // Act
        var data = await _testEntityService.GetAllAsync();

        // Assert
        Assert.Equal(2, data.Count());
    }

    [Fact]
    public void FirstOrDefaultTest()
    {
        // Arrange
        var entity = new TestEntity() { Value = "Foo" };
        _testEntityService.Add(entity);
        _ctx.SaveChanges();

        // Act
        var result = _testEntityService.FirstOrDefault(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Foo", result.Value);
    }
}