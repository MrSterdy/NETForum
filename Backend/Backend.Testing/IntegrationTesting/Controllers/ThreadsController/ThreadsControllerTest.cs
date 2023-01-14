namespace Backend.Testing.IntegrationTesting.Controllers.ThreadsController;

public abstract class ThreadsControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Threads";

    protected ThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
}