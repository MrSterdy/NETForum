using Backend.Core.Models.Comment;

using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.CommentsController;

public abstract class CommentsControllerTest : ControllerTest
{
    protected readonly Faker<CommentRequest> CommentGenerator = new Faker<CommentRequest>()
        .CustomInstantiator(faker => new CommentRequest(
            faker.Random.Int(),
            faker.Lorem.Paragraph()
        ));
    
    protected override string Endpoint => base.Endpoint + "/Comments";

    protected CommentsControllerTest(BackendFactory factory) : base(factory)
    {
    }
}