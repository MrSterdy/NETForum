namespace Backend.Testing.IntegrationTesting.Controllers.ThreadController;

public abstract class ThreadControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Thread";

    protected ThreadControllerTest(BackendFactory factory) : base(factory)
    {
    }
}