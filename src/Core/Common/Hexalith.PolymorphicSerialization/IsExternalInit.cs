// <copyright file="IsExternalInit.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace System.Runtime.CompilerServices;
#pragma warning restore IDE0130 // Namespace does not match folder structure

using System.ComponentModel;

/// <summary>
/// Bug fix for C# 9.0 init only setters in records.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public record IsExternalInit;