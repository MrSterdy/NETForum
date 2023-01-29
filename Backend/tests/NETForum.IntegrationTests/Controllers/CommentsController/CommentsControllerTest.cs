using NETForum.Models.Requests;

using Bogus;

namespace NETForum.IntegrationTests.Controllers.CommentsController;

public abstract class CommentsControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Comments";
    
    protected readonly Faker<CommentRequest> CommentGenerator = new Faker<CommentRequest>()
        .RuleFor(r => r.Content, faker => faker.Lorem.Paragraph());

    protected CommentsControllerTest(BackendFactory factory) : base(factory)
    {
    }
}