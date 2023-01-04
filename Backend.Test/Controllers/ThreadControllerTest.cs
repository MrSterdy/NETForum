using Backend.Controllers;
using Thread = Backend.Database.Entities.Thread;
using Backend.Database.Repositories;

using Moq;

namespace Backend.Test.Controllers;

public class ThreadControllerTest
{
    private readonly ThreadController _instance;

    public ThreadControllerTest()
    {
        var repoMock = new Mock<IThreadRepository>();
        repoMock.Setup(r => r.GetByIdAsync(0))
            .ReturnsAsync(new Thread(0, "How to prevent soap from dropping in the jail: 10 efficient methods", "10) ..."));

        _instance = new ThreadController(repoMock.Object);
    }

    [Fact]
    public async void TestGetByIdAsync()
    {
        var result = await _instance.GetByIdAsync(0);
        
        Assert.NotNull(result);
        
        Assert.Equal(0, result.Id);
        Assert.Equal(0, result.UserId);
        
        Assert.StartsWith("How to prevent", result.Name);
        Assert.StartsWith("10)", result.Content);
    }
}