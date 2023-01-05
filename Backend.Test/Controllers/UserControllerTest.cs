using Backend.Controllers;
using Backend.Database.Entities;
using Backend.Database.Repositories;

namespace Backend.Test.Controllers;

public class UserControllerTest
{
    private readonly UserController _instance;

    public UserControllerTest()
    {
        var ctx = DatabaseHelper.CreateContext();
        ctx.Users.Add(new User("MrSterdy"));
        ctx.SaveChanges();

        _instance = new UserController(new UserRepository(ctx));
    }

    [Fact]
    public async void GetByIdAsync_ValidId_Success()
    {
        var result = await _instance.GetByIdAsync(1);
        
        Assert.NotNull(result);
        
        Assert.Equal(1, result.Id);
        Assert.Equal("MrSterdy", result.Username);
    }
    
    [Fact]
    public async void GetByIdAsync_InvalidId_Null()
    {
        var result = await _instance.GetByIdAsync(-1);
        
        Assert.Null(result);
    }
}