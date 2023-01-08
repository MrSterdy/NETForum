using Backend.Core.Controllers;
using Backend.Core.Database.Entities;
using Backend.Core.Database.Repositories;
using Thread = Backend.Core.Database.Entities.Thread;

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
        
        Assert.Single(result.Items);
        
        Assert.True(result.IsLast);
        
        var thread = result.Items.First();

        Assert.Equal(1, thread.Id);
        Assert.Equal(1, thread.User.Id);
    }
    
    [Fact]
    public async void GetByPageAsync_InvalidPage_Empty()
    {
        var result = await _instance.GetByPageAsync(-1);
        
        Assert.Empty(result.Items);
    }

    [Fact]
    public async void GetByUserId_ValidId_ValidPage_Success()
    {
        var result = await _instance.GetByUserIdAsync(1, 1);
        
        Assert.Single(result.Items);

        Assert.True(result.IsLast);

        var thread = result.Items.First();
        
        Assert.Equal(1, thread.Id);
        Assert.Equal(1, thread.User.Id);
    }
}