// <copyright file="InvalidRequestChunkException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System;

/// <summary>
/// Exception thrown when an invalid request chunk is encountered.
/// </summary>
public class InvalidRequestChunkException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidRequestChunkException"/> class.
    /// </summary>
    public InvalidRequestChunkException()
        : base("Cannot create next chunk request if Take is not set or previous result is null.")
    {
    }
}