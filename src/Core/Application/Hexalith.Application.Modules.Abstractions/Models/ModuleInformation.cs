// <copyright file="ModuleInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Modules.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents information about a module.
/// </summary>
/// <param name="Name">The name of the module.</param>
/// <param name="Description">The description of the module.</param>
/// <param name="Version">The version of the module.</param>
/// <param name="IsApplicationModule">Indicates whether the module is an application module.</param>
[DataContract]
public record ModuleInformation(

    [property: DataMember(Order = 1)]
    string Name,

    [property: DataMember(Order = 2)]
    string Description,

    [property: DataMember(Order = 3)]
    string Version,

    [property: DataMember(Order = 4)]
    bool IsApplicationModule)
{
}