namespace Backend.Testing.IntegrationTesting.Controllers.CommentsController;

public abstract class CommentsControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Comments";

    protected CommentsControllerTest(BackendFactory factory) : base(factory)
    {
    }
}