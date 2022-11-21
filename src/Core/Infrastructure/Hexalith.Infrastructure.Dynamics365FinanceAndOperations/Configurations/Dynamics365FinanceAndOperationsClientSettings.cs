// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.AzureActiveDirectory.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;

/// <summary>
/// Dynamics 365 Finance and Operations simple ODATA Client
/// </summary>
public class Dynamics365FinanceAndOperationsClientSettings : ISettings
{
	/// <summary>
	/// Microsoft Dynamics 365 for finance and operations company identifier (DataAreaId).
	/// </summary>
	public string? Company { get; set; }

	/// <summary>
	/// Application Azure Active Directory identity
	/// </summary>
	public AzureActiveDirectoryApplicationSecurityContextConfiguration? Identity { get; set; }

	/// <summary>
	/// Microsoft Dynamics 365 for finance and operations instance url (https://xxx-devaos.axcloud.dynamics.com/).
	/// </summary>
	public Uri? Instance { get; set; }

	public static string ConfigurationName() => nameof(Dynamics365FinanceAndOperationsClient);
}