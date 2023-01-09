namespace Backend.Testing.IntegrationTesting.Controllers.UserController;

public abstract class UserControllerTest : ControllerTest
{
    protected override string Endpoint => "api/user";

    protected UserControllerTest(BackendFactory factory) : base(factory)
    {
    }
}