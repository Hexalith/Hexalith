// <copyright file="ApplicationModuleAddServicesException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;

/// <summary>
/// Exception that is thrown when there is a failure to add services from an application module.
/// </summary>
public class ApplicationModuleAddServicesException(string? message, Exception? innerException) : Exception($"Failed to load services from {message} module.", innerException)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationModuleAddServicesException"/> class.
    /// </summary>
    public ApplicationModuleAddServicesException()
        : this(null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationModuleAddServicesException"/> class
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The name of the module that failed to load services.</param>
    public ApplicationModuleAddServicesException(string? message)
        : this(message, null)
    {
    }
}