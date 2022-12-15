// <copyright file="Dynamics365FinanceAndOperationsClientSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.AzureActiveDirectory.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

/// <summary>
/// Dynamics 365 Finance and Operations simple ODATA Client.
/// </summary>
public class Dynamics365FinanceAndOperationsClientSettings : ISettings
{
    /// <summary>
    /// Gets or sets Microsoft Dynamics 365 for finance and operations company identifier (DataAreaId).
    /// </summary>
    /// <value>
    /// Microsoft Dynamics 365 for finance and operations company identifier (DataAreaId).
    /// </value>
    public string? Company { get; set; }

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
    public static string ConfigurationName()
    {
        return nameof(Dynamics365FinanceAndOperationsClient);
    }
}