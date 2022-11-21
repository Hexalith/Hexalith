// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Infrastructure.AzureActiveDirectory.Configurations;

/// <summary>
/// Azure Active Directory Application Configuration
/// </summary>
public class AzureActiveDirectoryApplicationSecurityContextConfiguration
{
	/// <summary>
	/// Azure Active Directory Application identifier (ClientId).
	/// </summary>
	public string? ApplicationId { get; set; }

	/// <summary>
	/// Azure Active Directory Application secret.
	/// </summary>
	public string? ApplicationSecret { get; set; }

	/// <summary>
	/// Microsoft Azure Active Directory tenant identifier (for example: yourdomain.com).
	/// </summary>
	public string? Tenant { get; set; }
}