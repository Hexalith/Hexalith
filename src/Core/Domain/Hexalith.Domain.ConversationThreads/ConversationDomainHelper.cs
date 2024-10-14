// <copyright file="ConversationDomainHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ConversationThreads;

using Hexalith.Domain.ConversationThreads.Aggregates;

public static class ConversationDomainHelper
{
    public static string ConversationThreadAggregateName => nameof(ConversationThread);
}