// <copyright file="FluentValidateOptions.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Configuration;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Implementation of <see cref="IValidateOptions{TOptions}"/> that uses FluentValidation <see cref="Validator"/> for validation.
/// </summary>
/// <typeparam name="TOptions">The instance being validated.</typeparam>
public partial class FluentValidateOptions<TOptions>
    : IValidateOptions<TOptions>
    where TOptions : class
{
    private readonly ILogger<FluentValidateOptions<TOptions>> _logger;
    private readonly IValidator<TOptions>? _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentValidateOptions{T}"/> class.
    /// Constructor.
    /// </summary>
    /// <param name="name">The name of the option.</param>
    /// <param name="provider">The service provider.</param>
    public FluentValidateOptions(string? name, [NotNull] IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        Name = name;
        _validator = provider.GetService(typeof(IValidator<TOptions>)) as IValidator<TOptions>;
        _logger = provider.GetRequiredService<ILogger<FluentValidateOptions<TOptions>>>();
    }

    /// <summary>
    /// Gets the options name.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Validates a specific named options instance (or all when <paramref name="name"/> is null).
    /// </summary>
    /// <param name="name">The name of the options instance being validated.</param>
    /// <param name="options">The options instance.</param>
    /// <returns>The <see cref="ValidateOptionsResult"/> result.</returns>
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        // Null name is used to configure all named options.
        if (Name != null && Name != name)
        {
            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }

        // The fluent validator is missing.
        if (_validator == null)
        {
            ValidatorNotFound(typeof(IValidator<TOptions>).Name);

            // Ignored if not validating this instance.
            return ValidateOptionsResult.Skip;
        }

        // Ensure options are provided to validate against
        ArgumentNullException.ThrowIfNull(options);
        FluentValidation.Results.ValidationResult results = _validator.Validate(options);
        if (results.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        List<string> errors = [];
        string typeName = options.GetType().Name;
        foreach (FluentValidation.Results.ValidationFailure? result in results.Errors)
        {
            errors.Add($"Validation failed for '{typeName}': '{string.Join(",", result.AttemptedValue)}' with the error: '{result.ErrorMessage}'.");
        }

        return ValidateOptionsResult.Fail(errors);
    }

    /// <summary>
    /// Logs a warning message when the validator is not found in the dependency injection container.
    /// </summary>
    /// <param name="validatorName">The name of the validator.</param>
    [LoggerMessage(
    EventId = 1,
    Level = LogLevel.Warning,
    Message = "Validator '{ValidatorName}' not found in dependency injection container.")]
    public partial void ValidatorNotFound(string validatorName);
}