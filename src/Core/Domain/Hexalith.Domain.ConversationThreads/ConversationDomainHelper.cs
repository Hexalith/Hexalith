// <copyright file="ConversationDomainHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ConversationThreads;

using Hexalith.Domain.ConversationThreads.Aggregates;

/// <summary>
/// Provides helper methods and properties for conversation domain.
/// </summary>
public static class ConversationDomainHelper
{
    /// <summary>
    /// Gets the name of the conversation thread aggregate.
    /// </summary>
    /// <value>
    /// The name of the conversation thread aggregate.
    /// </value>
    public static string ConversationThreadDomainName => nameof(ConversationThread);
}