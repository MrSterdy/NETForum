namespace Backend.Testing.IntegrationTesting.Controllers.AuthController;

public abstract class AuthControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Auth";

    protected AuthControllerTest(BackendFactory factory) : base(factory)
    {
    }
}