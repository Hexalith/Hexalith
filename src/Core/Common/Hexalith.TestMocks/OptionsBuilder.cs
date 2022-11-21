// Fiveforty S.A. Paris France (2022)
namespace Hexalith.TestMocks;

using Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Moq;

public class OptionsBuilder<T> where T : class, ISettings
{
	private T? _value;

	public bool HasValue => _value is not null;

	public IOptions<T> Build() => BuildMock().Object;

	public Mock<IOptions<T>> BuildMock()
	{
		Mock<IOptions<T>> mock = new();
		if (_value is not null)
		{
			_ = mock
				.Setup(x => x.Value)
				.Returns(_value);
		}
		return mock;
	}

	public OptionsBuilder<T> WithValue(T value)
	{
		_value = value;
		return this;
	}

	public OptionsBuilder<T> WithValueFromConfiguration<TProgram>() where TProgram : class
	{
		IConfigurationBuilder builder = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
			.AddUserSecrets<TProgram>();

		IConfigurationRoot configuration = builder.Build();
		_value = configuration
			.GetSection(T.ConfigurationName())
			.Get<T>() ?? throw new Exception("Unable to get settings: " + typeof(T).Name);
		return this;
	}
}