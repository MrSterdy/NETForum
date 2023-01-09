using Thread = Backend.Core.Database.Entities.Thread;

using Bogus;
using Bogus.Extensions;

using Microsoft.AspNetCore.Identity;

namespace Backend.Testing.Database;

public class Seeder
{
    private readonly Faker<IdentityUser<int>> _userGenerator = new Faker<IdentityUser<int>>()
        .RuleFor(u => u.UserName, faker => faker.Internet.UserName().ClampLength(4, 16))
        .RuleFor(u => u.NormalizedUserName, (_, u) => u.UserName!.ToUpper())
        .RuleFor(u => u.Email, faker => faker.Internet.Email())
        .RuleFor(u => u.NormalizedEmail, (_, u) => u.Email!.ToUpper())
        .RuleFor(u => u.SecurityStamp, () => Guid.NewGuid().ToString("D"))
        .RuleFor(u => u.PasswordHash,
            (_, u) => new PasswordHasher<IdentityUser<int>>().HashPassword(u, u.UserName!));

    private readonly Faker<Thread> _threadGenerator = new Faker<Thread>()
        .RuleFor(t => t.UserId, faker => faker.Random.Int())
        .RuleFor(t => t.Title, faker => faker.Lorem.Sentence())
        .RuleFor(t => t.Content, faker => faker.Lorem.Paragraph());

    private readonly TestContext _dbContext;

    public Seeder(TestContext context) =>
        _dbContext = context;
    
    public async Task<IdentityUser<int>> SeedUserAsync()
    {
        var result = await _dbContext.Users.AddAsync(_userGenerator.Generate());
        
        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<IdentityUser<int>> SeedVerifiedUserAsync()
    {
        var user = _userGenerator.Generate();
        user.EmailConfirmed = true;
        
        var result = await _dbContext.Users.AddAsync(user);
        
        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }
    
    public async Task<Thread> SeedThreadAsync()
    {
        var user = await SeedVerifiedUserAsync();

        var thread = _threadGenerator.Generate();
        thread.UserId = user.Id;

        var result = await _dbContext.Threads.AddAsync(thread);

        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }
}