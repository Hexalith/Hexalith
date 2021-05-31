namespace Hexalith.Infrastructure.InMemory.Tests
{
    using System.Threading.Tasks;

    using Hexalith.Application.Abstractions.Tests;
    using Hexalith.Application.Abstractions.Tests.Fixtures;
    using Hexalith.Application.Events;
    using Hexalith.Application.Repositories;
    using Hexalith.Infrastructure.InMemory.Repositories;

    using Moq;

    using Xunit;

    public class InMemoryRepositoryTest : RepositoryTestBase
    {
        [Fact]
        protected override Task Check_save_state_to_stream()
        {
            return base.Check_save_state_to_stream();
        }

        [Fact]
        protected override Task Check_set_new_state()
        {
            return base.Check_set_new_state();
        }

        [Fact]
        protected override Task Check_update_state()
        {
            return base.Check_update_state();
        }

        protected override IRepository CreateNewRepository()
                    => new InMemoryRepository<IDummyState, DummyState>(new Mock<IEventBus>().Object);

        [Fact]
        protected override Task Save_state_throws_not_supported()
        {
            return base.Save_state_throws_not_supported();
        }

        [Fact]
        protected override Task Save_stream_throws_not_supported()
        {
            return base.Save_stream_throws_not_supported();
        }
    }
}