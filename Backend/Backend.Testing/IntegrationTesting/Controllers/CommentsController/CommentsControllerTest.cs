using Backend.Core.Models.Comment;

using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.CommentsController;

public abstract class CommentsControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Comments";
    
    protected readonly Faker<CommentRequest> CommentGenerator = new Faker<CommentRequest>()
        .RuleFor(r => r.Content, faker => faker.Lorem.Paragraph());

    protected CommentsControllerTest(BackendFactory factory) : base(factory)
    {
    }
}