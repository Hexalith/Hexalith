// <copyright file="MemoryFalseTrueFieldValueDisabledService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Services;

using System.Threading.Tasks;

/// <summary>
/// Represents a service for retrieving the disabled false or true values of a field.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MemoryFalseTrueFieldValueDisabledService"/> class.
/// </remarks>
/// <param name="falseDisabled">A value indicating whether the false value is disabled.</param>
/// <param name="trueDisabled">A value indicating whether the true value is disabled.</param>
public class MemoryFalseTrueFieldValueDisabledService(bool falseDisabled, bool trueDisabled) : IFalseTrueFieldValueDisabledService
{
    private readonly bool _falseDisabled = falseDisabled;
    private readonly bool _trueDisabled = trueDisabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryFalseTrueFieldValueDisabledService"/> class.
    /// </summary>
    public MemoryFalseTrueFieldValueDisabledService()
        : this(false, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryFalseTrueFieldValueDisabledService"/> class.
    /// </summary>
    /// <param name="values">A tuple containing the false and true disabled values.</param>
    public MemoryFalseTrueFieldValueDisabledService((bool FalseDisabled, bool TrueDisabled) values)
        : this(values.FalseDisabled, values.TrueDisabled)
    {
    }

    /// <inheritdoc/>
    public Task<(bool FalseDisabled, bool TrueDisabled)> FalseTrueFieldDisabledValuesAsync(
        string fieldId,
        CancellationToken cancellationToken)
            => Task.FromResult((_falseDisabled, _trueDisabled));
}