namespace NETForum.IntegrationTests.Controllers.UsersController;

public abstract class UsersControllerTest : ControllerTest
{
    protected override string Endpoint => base.Endpoint + "/Users";

    protected UsersControllerTest(BackendFactory factory) : base(factory)
    {
    }
}