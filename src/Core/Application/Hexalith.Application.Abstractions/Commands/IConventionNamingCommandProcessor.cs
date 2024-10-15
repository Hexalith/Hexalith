// <copyright file="IConventionNamingCommandProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

/// <summary>
/// Defines the interface for a command processor that uses convention-based naming.
/// </summary>
/// <remarks>
/// This interface extends <see cref="IDomainCommandProcessor"/> and is used to process commands
/// based on naming conventions. Implementations of this interface should provide logic
/// for handling commands where the naming follows specific conventions.
/// </remarks>
public interface IConventionNamingCommandProcessor : IDomainCommandProcessor
{
    // Interface members can be added here if needed in the future
}
