﻿// <copyright file="ISearchChunkableRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Interface for chunkable requests.
/// </summary>
public interface ISearchChunkableRequest : IChunkableRequest, ISearchRequest
{
}