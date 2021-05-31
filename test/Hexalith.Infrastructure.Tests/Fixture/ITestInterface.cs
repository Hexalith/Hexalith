namespace Hexalith.Infrastructure.Tests.Fixture
{
    public interface ITestInterface
    {
    }

    public interface ITestInterface<T> : ITestInterface
    {
    }

    public interface ITestInterfaceNoClass
    {
    }

    public class TestConcrete1 : ITestInterface
    {
    }

    public class TestConcrete2 : ITestInterface
    {
    }

    public class TestConcrete3 : ITestInterface
    {
    }

    public class TestConcrete4 : ITestInterface<int>
    {
    }

    public class TestConcrete5 : ITestInterface<string>
    {
    }

    public class TestConcrete6 : TestConcrete5
    {
    }
}