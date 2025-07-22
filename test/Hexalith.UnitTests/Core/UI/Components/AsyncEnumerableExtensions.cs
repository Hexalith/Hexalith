// <copyright file="AsyncEnumerableExtensions.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.UI.Components;

public static class AsyncEnumerableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        List<T> list = [];
        await foreach (T item in source.WithCancellation(cancellationToken))
        {
            list.Add(item);
        }

        return list;
    }
}