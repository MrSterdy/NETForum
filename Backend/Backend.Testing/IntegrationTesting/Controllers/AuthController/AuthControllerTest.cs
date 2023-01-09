namespace Backend.Testing.IntegrationTesting.Controllers.AuthController;

public abstract class AuthControllerTest : ControllerTest
{
    protected override string Endpoint => "api/auth";

    protected AuthControllerTest(BackendFactory factory) : base(factory)
    {
    }
}