// <copyright file="PartitionSettings.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Partitions.Configurations;

using System.Runtime.Serialization;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Represents the settings for a partition.
/// </summary>
[DataContract]
public class PartitionSettings : ISettings
{
    /// <summary>
    /// Gets or sets the default partition name.
    /// </summary>
    public string Default { get; set; } = nameof(Hexalith);

    /// <summary>
    /// Gets the configuration name for the partition settings.
    /// </summary>
    /// <returns>The configuration name.</returns>
    public static string ConfigurationName() => $"{nameof(Hexalith)}:{nameof(Partitions)}";
}