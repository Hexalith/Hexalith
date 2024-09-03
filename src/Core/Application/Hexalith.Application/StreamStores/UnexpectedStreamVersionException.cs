// <copyright file="UnexpectedStreamVersionException.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using System.Globalization;
using System.Runtime.Serialization;

/// <summary>
/// Exception thrown when a stream version is not the expected one.
/// </summary>
[DataContract]
public class UnexpectedStreamVersionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
    /// </summary>
    public UnexpectedStreamVersionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public UnexpectedStreamVersionException(string? message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
    /// </summary>
    /// <param name="expectedVersion">The expected version of the stream.</param>
    /// <param name="actualVersion">The actual version of the stream.</param>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">Inner exception.</param>
    public UnexpectedStreamVersionException(
    long expectedVersion,
    long actualVersion,
    string? message,
    Exception? innerException)
    : base(string.Create(CultureInfo.InvariantCulture, $"Unexpected stream version '{expectedVersion}'. Actual version : '{actualVersion}'. ") + message, innerException)
    {
        ExpectedVersion = expectedVersion;
        ActualVersion = actualVersion;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
    /// </summary>
    /// <param name="expectedVersion">The expected version of the stream.</param>
    /// <param name="actualVersion">The actual version of the stream.</param>
    public UnexpectedStreamVersionException(
    long expectedVersion,
    long actualVersion)
    : this(expectedVersion, actualVersion, message: null, innerException: null)
    {
        ExpectedVersion = expectedVersion;
        ActualVersion = actualVersion;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedStreamVersionException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">Inner exception.</param>
    public UnexpectedStreamVersionException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the actual version of the stream.
    /// </summary>
    public long ActualVersion { get; private set; }

    /// <summary>
    /// Gets the expected version of the stream.
    /// </summary>
    public long ExpectedVersion { get; private set; }
}