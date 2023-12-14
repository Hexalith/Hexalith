// <copyright file="Dynamics365FinanceClientSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Configurations;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.AzureActiveDirectory.Configurations;

/// <summary>
/// Dynamics 365 Finance and Operations simple ODATA Client.
/// </summary>
public class Dynamics365FinanceClientSettings : ISettings
{
    /// <summary>
    /// Gets or sets application Azure Active Directory identity.
    /// </summary>
    /// <value>
    /// Application Azure Active Directory identity.
    /// </value>
    public AzureActiveDirectoryApplicationSecurityContextConfiguration? Identity { get; set; }

    /// <summary>
    /// Gets or sets Microsoft Dynamics 365 for finance and operations instance URL (https://xxx-devaos.axcloud.dynamics.com/).
    /// </summary>
    /// <value>
    /// Microsoft Dynamics 365 for finance and operations instance URL (https://xxx-devaos.axcloud.dynamics.com/).
    /// </value>
    public Uri? Instance { get; set; }

    /// <summary>
    /// Gets or sets the settings name..
    /// </summary>
    /// <returns>The name.</returns>
    public static string ConfigurationName() => nameof(Dynamics365Finance);
}