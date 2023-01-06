using Backend.Controllers;
using Backend.Database.Entities;
using Backend.Database.Repositories;
using Thread = Backend.Database.Entities.Thread;

namespace Backend.Test.Controllers;

public class ThreadControllerTest
{
    private readonly ThreadController _instance;

    public ThreadControllerTest()
    {
        var ctx = DatabaseHelper.CreateContext();
        ctx.Users.Add(new User("MrSterdy"));
        ctx.Threads.Add(new Thread(1, "TestTitle", "TestContent"));
        ctx.SaveChanges();
        
        _instance = new ThreadController(new ThreadRepository(ctx));
    }

    [Fact]
    public async void GetByIdAsync_ValidId_Success()
    {
        var result = await _instance.GetByIdAsync(1);
        
        Assert.NotNull(result);
        
        Assert.Equal(1, result.Id);
        Assert.Equal(1, result.UserId);
        Assert.Equal("MrSterdy", result.User.Username);
        Assert.Equal("TestTitle", result.Title);
        Assert.Equal("TestContent", result.Content);
    }
    
    [Fact]
    public async void GetByIdAsync_InvalidId_Null()
    {
        var result = await _instance.GetByIdAsync(-1);
        
        Assert.Null(result);
    }

    [Fact]
    public async void GetByPageAsync_ValidPage_Success()
    {
        var result = await _instance.GetByPageAsync(1);
        
        Assert.Single(result.Entities);
        
        Assert.True(result.IsLast);
    }
    
    [Fact]
    public async void GetByPageAsync_InvalidPage_Empty()
    {
        var result = await _instance.GetByPageAsync(-1);
        
        Assert.Empty(result.Entities);
    }

    [Fact]
    public async void GetByUserId_ValidId_ValidPage_Success()
    {
        var result = await _instance.GetByUserIdAsync(1, 1);
        
        Assert.Single(result.Entities);

        Assert.True(result.IsLast);
        
        Assert.Equal(1, result.Entities.First().Id);
        Assert.Equal(1, ((Thread) result.Entities.First()).UserId);
    }
}