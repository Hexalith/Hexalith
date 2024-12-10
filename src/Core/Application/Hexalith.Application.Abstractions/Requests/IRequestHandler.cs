// <copyright file="IIntegrationRequestHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;

public interface IRequestHandler
{
    Task<object> ExecuteAsync(object baseRequest, Metadata metadata, CancellationToken cancellationToken);
}