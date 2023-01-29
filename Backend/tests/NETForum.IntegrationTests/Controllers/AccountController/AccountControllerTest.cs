namespace NETForum.IntegrationTests.Controllers.AccountController;

public abstract class AccountControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Account";

    protected AccountControllerTest(BackendFactory factory) : base(factory)
    {
    }
}