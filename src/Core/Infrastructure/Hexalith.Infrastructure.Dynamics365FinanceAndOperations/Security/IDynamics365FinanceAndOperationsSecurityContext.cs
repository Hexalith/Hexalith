// <copyright file="IDynamics365FinanceAndOperationsSecurityContext.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Hexalith.Infrastructure.Security.Abstractions;

public interface IDynamics365FinanceAndOperationsSecurityContext : IApplicationSecurityContext
{
	Task<string> AcquireToken(CancellationToken cancellationToken);
}