// <copyright file="EquatableHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

using System.Collections;
using System.Linq;

using Hexalith.Extensions.Common;

/// <summary>
/// Class EquatableHelper.
/// </summary>
public static class EquatableHelper
{
    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSame(this object? a, object? b)
    {
        if (a is null)
        {
            return b is null;
        }

        if (b is null)
        {
            return false;
        }

        if (a.Equals(b))
        {
            return true;
        }

        if (a.GetType() != b.GetType())
        {
            return false;
        }

        if (a is IDictionary aDictionary)
        {
            return AreSameDictionary(aDictionary, (IDictionary)b);
        }

        if (a is IEnumerable aEnumerable)
        {
            return AreSameEnumeration(aEnumerable, (IEnumerable)b);
        }

        if (a is IEquatableObject aEquatable)
        {
            IEquatableObject bEquatable = (IEquatableObject)b;
            return AreSameEnumeration(aEquatable.GetEqualityComponents(), bEquatable.GetEqualityComponents());
        }

        return false;
    }

    /// <summary>
    /// Ares the same dictionary.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSameDictionary(this IDictionary? a, IDictionary? b)
    {
        if (a is null)
        {
            return b is null;
        }

        if (b is null)
        {
            return false;
        }

        object?[] aKeys = a.Keys.Cast<object?>().ToArray();
        object?[] aValues = a.Values.Cast<object?>().ToArray();
        object?[] bKeys = b.Keys.Cast<object?>().ToArray();
        object?[] bValues = b.Values.Cast<object?>().ToArray();

        return AreSameEnumeration(aKeys, bKeys) && AreSameEnumeration(aValues, bValues);
    }

    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSameEnumeration(this IEnumerable? a, IEnumerable? b)
    {
        if (a is null)
        {
            return b is null;
        }

        if (b is null)
        {
            return false;
        }

        object?[]? aArray = a.Cast<object?>().ToArray();
        object?[]? bArray = b.Cast<object?>().ToArray();
        if (aArray is null)
        {
            return bArray is null;
        }

        if (bArray is null)
        {
            return false;
        }

        if (aArray.Length != bArray.Length)
        {
            return false;
        }

        for (int i = 0; i < aArray.Length; i++)
        {
            if (!AreSame(aArray[i], bArray[i]))
            {
                return false;
            }
        }

        return true;
    }
}