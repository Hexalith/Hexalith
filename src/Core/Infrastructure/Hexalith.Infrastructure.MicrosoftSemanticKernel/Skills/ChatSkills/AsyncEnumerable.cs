namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

internal static class AsyncEnumerable
{
    public static async ValueTask<bool> ContainsAsync<T>(this IAsyncEnumerable<T> source, T value)
    {
        await foreach (T item in source.ConfigureAwait(continueOnCapturedContext: false))
        {
            if (EqualityComparer<T>.Default.Equals(item, value))
            {
                return true;
            }
        }

        return false;
    }

    public static async ValueTask<int> CountAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default(CancellationToken))
    {
        int count = 0;
        await foreach (T item in source.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
        {
            _ = item;
            count = checked(count + 1);
        }

        return count;
    }

    public static IAsyncEnumerable<T> Empty<T>()
    {
        return EmptyAsyncEnumerable<T>.Instance;
    }

    public static async ValueTask<T?> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default(CancellationToken))
    {
        ConfiguredCancelableAsyncEnumerable<T>.Enumerator enumerator = source.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext: false).GetAsyncEnumerator();
        try
        {
            if (await enumerator.MoveNextAsync())
            {
                return enumerator.Current;
            }
        }
        finally
        {
            IAsyncDisposable asyncDisposable = enumerator as IAsyncDisposable;
            if (asyncDisposable != null)
            {
                await asyncDisposable.DisposeAsync();
            }
        }

        return default(T);
    }

    public static async ValueTask<T?> LastOrDefaultAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default(CancellationToken))
    {
        T last = default(T);
        bool hasLast = false;
        await foreach (T item in source.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
        {
            hasLast = true;
            last = item;
        }

        return hasLast ? last : default(T);
    }

    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> source)
    {
        foreach (T item in source)
        {
            yield return item;
        }
    }

    public static IEnumerable<T> ToEnumerable<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default(CancellationToken))
    {
        IAsyncEnumerator<T> enumerator = source.GetAsyncEnumerator(cancellationToken);
        try
        {
            while (enumerator.MoveNextAsync().AsTask().GetAwaiter()
                .GetResult())
            {
                yield return enumerator.Current;
            }
        }
        finally
        {
            enumerator.DisposeAsync().AsTask().GetAwaiter()
                .GetResult();
        }
    }

    public static async ValueTask<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default(CancellationToken))
    {
        List<T> result = new List<T>();
        await foreach (T item in source.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
        {
            result.Add(item);
        }

        return result;
    }

    private sealed class EmptyAsyncEnumerable<T> : IAsyncEnumerable<T>, IAsyncEnumerator<T>, IAsyncDisposable
    {
        public static readonly EmptyAsyncEnumerable<T> Instance = new EmptyAsyncEnumerable<T>();

        public T Current => default(T);

        public ValueTask DisposeAsync()
        {
            return default(ValueTask);
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
        {
            return this;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(result: false);
        }
    }
}