using Backend.Core.Identity;
using Thread = Backend.Core.Database.Entities.Thread;

using Bogus;
using Bogus.Extensions;

using Microsoft.AspNetCore.Identity;

namespace Backend.Testing.Database;

public class Seeder
{
    private readonly Faker<ApplicationUser> _userGenerator = new Faker<ApplicationUser>()
        .RuleFor(u => u.UserName, faker => faker.Internet.UserName().ClampLength(4, 16))
        .RuleFor(u => u.NormalizedUserName, (_, u) => u.UserName!.ToUpper())
        .RuleFor(u => u.Email, faker => faker.Internet.Email())
        .RuleFor(u => u.NormalizedEmail, (_, u) => u.Email!.ToUpper())
        .RuleFor(u => u.SecurityStamp, () => Guid.NewGuid().ToString("D"))
        .RuleFor(u => u.PasswordHash,
            (_, u) => new PasswordHasher<ApplicationUser>().HashPassword(u, u.UserName!));

    private readonly Faker<Thread> _threadGenerator = new Faker<Thread>()
        .RuleFor(t => t.Title, faker => faker.Lorem.Sentence())
        .RuleFor(t => t.Content, faker => faker.Lorem.Paragraph());

    private readonly TestContext _dbContext;

    public Seeder(TestContext context) =>
        _dbContext = context;
    
    public async Task<ApplicationUser> SeedUserAsync()
    {
        var result = await _dbContext.Users.AddAsync(_userGenerator.Generate());
        
        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }

    public async Task<ApplicationUser> SeedVerifiedUserAsync()
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

    public async Task<Comment> SeedCommentAsync()
    {
        var thread = await SeedThreadAsync();

        var comment = _commentGenerator.Generate();
        comment.ThreadId = thread.Id;
        comment.UserId = thread.UserId;

        var result = await _dbContext.Comments.AddAsync(comment);

        await _dbContext.SaveChangesAsync();

        return result.Entity;
    }
}