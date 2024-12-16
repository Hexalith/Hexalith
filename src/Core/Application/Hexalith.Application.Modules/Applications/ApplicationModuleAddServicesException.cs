// <copyright file="ApplicationModuleAddServicesException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Applications;

using System;

/// <summary>
/// Exception that is thrown when there is a failure to add services from an application module.
/// </summary>
[Serializable]
internal class ApplicationModuleAddServicesException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationModuleAddServicesException"/> class.
    /// </summary>
    public ApplicationModuleAddServicesException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationModuleAddServicesException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The name of the module that failed to load services.</param>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    public ApplicationModuleAddServicesException(string? message, Exception? innerException)
        : base($"Failed to load services from {message} module.", innerException)
    {
    }
}