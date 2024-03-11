// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 12-22-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-22-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceInventoriesSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.Configuration;

using System;
using System.Runtime.Serialization;

using Hexalith.Extensions.Configuration;

/// <summary>
/// Class Dynamics365FinancePartiesSettings.
/// Implements the <see cref="ISettings" />.
/// </summary>
/// <seealso cref="ISettings" />
[Serializable]
[DataContract]
public class Dynamics365FinanceInventoriesSettings : ISettings
{
    /// <summary>
    /// The configuration section name of the settings.
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => nameof(Dynamics365Finance);
}