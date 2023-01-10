namespace Backend.Testing.IntegrationTesting.Controllers.UserController;

public abstract class UserControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/User";

    protected UserControllerTest(BackendFactory factory) : base(factory)
    {
    }
}