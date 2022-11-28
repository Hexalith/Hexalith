// <copyright file="IDynamics365FinanceAndOperationsSecurityContext.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Hexalith.Infrastructure.Security.Abstractions;

/// <summary>
/// The Dynamics 365 Finance and Operations security context.
/// </summary>
public interface IDynamics365FinanceAndOperationsSecurityContext : IApplicationSecurityContext
{
	/// <summary>
	/// Acquire a new token with default Dynamics 365 Finance and Operations scopes.
	/// </summary>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>The Dynamics 365 security token.</returns>
	Task<string> AcquireTokenAsync(CancellationToken cancellationToken);
}