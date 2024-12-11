// <copyright file="OptionsBuilder.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Moq;

/// <summary>
/// Helper class to build a <see cref="IOptions{TOptions}"/> mock.
/// </summary>
/// <typeparam name="T">The settings object type.</typeparam>
public class OptionsBuilder<T> : IMockBuilder<IOptions<T>>
    where T : class, ISettings
{
    private T? _value;

    /// <summary>
    /// Gets a value indicating whether check if the options have been set..
    /// </summary>
    public bool HasValue => _value is not null;

    /// <summary>
    /// Build a <see cref="IOptions{TOptions}"/>.
    /// </summary>
    /// <returns>The mocked options instance.</returns>
    public IOptions<T> Build() => BuildMock().Object;

    /// <summary>
    /// Build.
    /// </summary>
    /// <returns>The options mock instance.</returns>
    public IMock<IOptions<T>> BuildMock()
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

    /// <summary>
    /// Define the options value to be used.
    /// </summary>
    /// <param name="value">Value to return in the mocked options.</param>
    /// <returns>The options builder.</returns>
    public OptionsBuilder<T> WithValue(T value)
    {
        _value = value;
        return this;
    }

    /// <summary>
    /// Defines the options value from application settings JSON file or .NET user secrets.
    /// </summary>
    /// <typeparam name="TProgram">The test class to define the .NET user secrets assembly.</typeparam>
    /// <returns>The options builder.</returns>
    /// <exception cref="Exception">Unable to get settings.</exception>
    public OptionsBuilder<T> WithValueFromConfiguration<TProgram>()
        where TProgram : class, new()
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddUserSecrets<TProgram>();

        string configurationName = T.ConfigurationName();
        IConfigurationRoot configuration = builder.Build();
        _value = configuration
            .GetSection(configurationName)
            .Get<T>()
            ?? throw new InvalidOperationException("Unable to get settings: " + configurationName);
        return this;
    }
}