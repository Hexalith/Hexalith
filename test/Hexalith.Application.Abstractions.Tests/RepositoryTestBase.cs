namespace Hexalith.Application.Abstractions.Tests
{
    using Hexalith.Application.Abstractions.Tests.Fixtures;
    using Hexalith.Application.Repositories;

    using FluentAssertions;

    using System;
    using System.Threading.Tasks;

    public abstract class RepositoryTestBase
    {
        protected NewDummyAdded new1 = new() { Value1 = true, Value2 = 10, Value3 = "Hello" };
        protected NewDummyAdded new2 = new() { Value1 = false, Value2 = 99, Value3 = "Cake" };
        protected NewDummyAdded new3 = new() { Value1 = true, Value2 = 1010, Value3 = "Sunday" };
        protected NewDummyAdded new4 = new() { Value1 = false, Value2 = 10900, Value3 = "June" };
        protected DummyState state1 = new() { Value1 = true, Value2 = 10, Value3 = "Hello" };
        protected DummyState state2 = new() { Value1 = false, Value2 = 99, Value3 = "Cake" };
        protected DummyState state3 = new() { Value1 = true, Value2 = 1010, Value3 = "Sunday" };
        protected DummyState state4 = new() { Value1 = false, Value2 = 10900, Value3 = "June" };
        protected RepositoryMetadata meta1 = new() { CorrelationId = "cor1", CausationId = "caus1", MessageId = "id1", UserName = "user1", UserDateTime = DateTimeOffset.Now.AddDays(1), SystemUtcDateTime = DateTime.UtcNow.AddHours(1) };
        protected RepositoryMetadata meta2 = new() { CorrelationId = "cor2", CausationId = "caus2", MessageId = "id2", UserName = "user2", UserDateTime = DateTimeOffset.Now.AddDays(2), SystemUtcDateTime = DateTime.UtcNow.AddHours(2) };
        protected RepositoryMetadata meta3 = new() { CorrelationId = "cor3", CausationId = "caus3", MessageId = "id3", UserName = "user3", UserDateTime = DateTimeOffset.Now.AddDays(3), SystemUtcDateTime = DateTime.UtcNow.AddHours(3) };
        protected RepositoryMetadata meta4 = new() { CorrelationId = "cor4", CausationId = "caus4", MessageId = "id4", UserName = "user4", UserDateTime = DateTimeOffset.Now.AddDays(4), SystemUtcDateTime = DateTime.UtcNow.AddHours(4) };

        protected virtual async Task Check_set_new_state()
        {
            var repository = CreateNewRepository();
            await repository.SetState(nameof(state1), meta1, state1, default);
            await repository.SetState(nameof(state2), meta2, state2, default);
            await repository.SetState(nameof(state3), meta3, state3, default);
            await repository.SetState(nameof(state4), meta4, state4, default);
            await repository.Save(default);
            CheckEqual(state1, (DummyState)await repository.GetState(typeof(DummyState), nameof(state1)));
            CheckEqual(state2, (DummyState)await repository.GetState(typeof(DummyState), nameof(state2)));
            CheckEqual(state3, (DummyState)await repository.GetState(typeof(DummyState), nameof(state3)));
            CheckEqual(state4, (DummyState)await repository.GetState(typeof(DummyState), nameof(state4)));
            CheckEqual(await repository.GetMetadata(nameof(state1)), meta1);
            CheckEqual(await repository.GetMetadata(nameof(state2)), meta2);
            CheckEqual(await repository.GetMetadata(nameof(state3)), meta3);
            CheckEqual(await repository.GetMetadata(nameof(state4)), meta4);
        }
        private static void CheckEqual(DummyState state1, DummyState state2)
        {
            state2.Value1.Should().Be(state1.Value1);
            state2.Value2.Should().Be(state1.Value2);
            state2.Value3.Should().Be(state1.Value3);
        }
        private static void CheckEqual(IRepositoryStateMetadata meta, IRepositoryMetadata createMeta, IRepositoryMetadata updateMeta = null)
        {
            meta.CreatedByUser.Should().Be(createMeta.UserName);
            meta.CreatedUtcDateTime.Should().Be(createMeta.SystemUtcDateTime);
            meta.LastModifiedByUser.Should().Be(updateMeta?.UserName);
            meta.LastModifiedUtcDateTime.Should().Be(updateMeta?.SystemUtcDateTime);
        }
        protected virtual async Task Check_update_state()
        {
            var repository = CreateNewRepository();
            await repository.SetState(nameof(state1), meta1, state1, default);
            await repository.SetState(nameof(state2), meta2, state2, default);
            await repository.Save(default);
            await repository.SetState(nameof(state1), meta3, state3, default);
            await repository.SetState(nameof(state2), meta4, state4, default);
            await repository.Save(default);
            CheckEqual(state1, (DummyState)await repository.GetState(typeof(DummyState), nameof(state3)));
            CheckEqual(state2, (DummyState)await repository.GetState(typeof(DummyState), nameof(state4)));
            CheckEqual(await repository.GetMetadata(nameof(state1)), meta1, meta3);
            CheckEqual(await repository.GetMetadata(nameof(state2)), meta2, meta4);
        }

        protected virtual Task Check_save_state_to_stream()
        {
            return Task.CompletedTask;
        }

        protected abstract IRepository CreateNewRepository();

        protected virtual Task Save_state_throws_not_supported()
        {
            return Task.CompletedTask;
        }

        protected virtual Task Save_stream_throws_not_supported()
        {
            return Task.CompletedTask;
        }
    }
}