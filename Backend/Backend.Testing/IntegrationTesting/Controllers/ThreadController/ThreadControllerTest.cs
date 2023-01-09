namespace Backend.Testing.IntegrationTesting.Controllers.ThreadController;

public abstract class ThreadControllerTest : ControllerTest
{
    protected override string Endpoint => "api/thread";

    protected ThreadControllerTest(BackendFactory factory) : base(factory)
    {
    }
}