using Backend.Core.Models.Thread;

using Bogus;

namespace Backend.Testing.IntegrationTesting.Controllers.ThreadsController;

public abstract class ThreadsControllerTest : ControllerTest
{
    protected readonly Faker<ThreadRequest> ThreadGenerator = new Faker<ThreadRequest>()
        .CustomInstantiator(faker => new ThreadRequest(
            faker.Lorem.Sentence(),
            faker.Lorem.Paragraph()
        ));

    protected override string Endpoint => base.Endpoint + "/Threads";

    protected ThreadsControllerTest(BackendFactory factory) : base(factory)
    {
    }
}