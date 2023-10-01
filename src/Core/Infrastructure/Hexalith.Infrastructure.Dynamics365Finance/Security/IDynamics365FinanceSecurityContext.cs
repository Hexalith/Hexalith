// <copyright file="IDynamics365FinanceSecurityContext.cs" company="Fiveforty SAS Paris France">
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

namespace Hexalith.Infrastructure.Dynamics365Finance.Security;

using Hexalith.Infrastructure.Security.Abstractions;

/// <summary>
/// The Dynamics 365 Finance and Operations security context.
/// </summary>
public interface IDynamics365FinanceSecurityContext : IApplicationSecurityContext
{
    /// <summary>
    /// Acquire a new token with default Dynamics 365 Finance and Operations scopes.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The Dynamics 365 security token.</returns>
    Task<string> AcquireTokenAsync(CancellationToken cancellationToken);
}