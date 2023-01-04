using Backend.Controllers;
using Backend.Database.Entities;
using Backend.Database.Repositories;

using Moq;

namespace Backend.Test.Controllers;

public class UserControllerTest
{
    private readonly UserController _instance;

    public UserControllerTest()
    {
        var repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetByIdAsync(0))
            .ReturnsAsync(new User("MrSterdy"));

        _instance = new UserController(repoMock.Object);
    }

    [Fact]
    public async void TestGetByIdAsync()
    {
        var result = await _instance.GetByIdAsync(0);
        
        Assert.NotNull(result);
        
        Assert.Equal(0, result.Id);
        Assert.Equal("MrSterdy", result.Username);
    }
}