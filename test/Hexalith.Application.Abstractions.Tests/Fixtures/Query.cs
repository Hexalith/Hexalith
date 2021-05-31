namespace Hexalith.Application.Abstractions.Tests.Fixture
{
    using Hexalith.Application.Queries;

    public class TestIdQuery : QueryBase<TestId, int>
    {
        public TestIdQuery(TestId id) : base(id)
        {
        }
    }

    public class TestQueryNoId : QueryBase<string>
    {
    }
}