// <copyright file="SessionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Helpers;

using System.Security.Claims;

/// <summary>
/// Provides helper methods for managing user sessions and roles.
/// </summary>
public static class SessionHelper
{
    /// <summary>
    /// The application claim prefix.
    /// </summary>
    public const string ApplicationClaimPrefix = "hexalith-";

    /// <summary>
    /// The global administrator role name.
    /// </summary>
    public const string GlobalAdministratorRole = ApplicationClaimPrefix + "administrator";

    /// <summary>
    /// The partition ID claim name.
    /// </summary>
    public const string PartitionIdClaimName = ApplicationClaimPrefix + "partition-id";

    /// <summary>
    /// The session ID claim name.
    /// </summary>
    public const string SessionIdClaimName = ApplicationClaimPrefix + "hexalith-session-id";

    /// <summary>
    /// Gets the contributor role name for the specified partition ID.
    /// </summary>
    /// <param name="partitionId">The partition ID.</param>
    /// <returns>The contributor role name.</returns>
    public static string ContributorRoleName(string partitionId)
        => ApplicationClaimPrefix + partitionId + "-contributor";

    /// <summary>
    /// Finds the partition ID claim value for the specified user.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>The partition ID claim value, or null if not found.</returns>
    public static string? FindPartitionId(this ClaimsPrincipal user) => user.FindFirst(PartitionIdClaimName)?.Value;

    /// <summary>
    /// Finds the session ID claim value for the specified user.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>The session ID claim value, or null if not found.</returns>
    public static string? FindSessionId(this ClaimsPrincipal user) => user.FindFirst(SessionIdClaimName)?.Value;

    /// <summary>
    /// Gets the partition ID claim value for the specified user.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>The partition ID claim value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the partition ID claim is not found.</exception>
    public static string GetPartitionId(this ClaimsPrincipal user)
        => user.FindPartitionId() ?? throw new InvalidOperationException($"Partition ID ({PartitionIdClaimName}) claim not found in user principal.");

    /// <summary>
    /// Gets the roles for the specified user.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>A collection of role names.</returns>
    public static IEnumerable<string>? GetRoles(this ClaimsPrincipal user)
                => user.FindAll(p => p.Type == ClaimTypes.Role).Select(p => p.Value);

    /// <summary>
    /// Gets the session ID claim value for the specified user.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>The session ID claim value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the session ID claim is not found.</exception>
    public static string GetSessionId(this ClaimsPrincipal user)
        => user.FindSessionId() ?? throw new InvalidOperationException($"Session ID ({SessionIdClaimName}) claim not found in user principal.");

    /// <summary>
    /// Determines whether the specified user is a contributor.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>True if the user is a contributor; otherwise, false.</returns>
    public static bool IsContributor(this ClaimsPrincipal user)
        => (user.GetRoles() ?? [])
            .Any(p => p == ContributorRoleName(user.GetPartitionId()));

    /// <summary>
    /// Determines whether the specified user is a global administrator.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>True if the user is a global administrator; otherwise, false.</returns>
    public static bool IsGlobalAdministrator(this ClaimsPrincipal user)
        => (user.GetRoles() ?? [])
        .Any(p => p == GlobalAdministratorRole);

    /// <summary>
    /// Determines whether the specified user is an owner.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>True if the user is an owner; otherwise, false.</returns>
    public static bool IsOwner(this ClaimsPrincipal user)
        => (user.GetRoles() ?? [])
            .Any(p => p == OwnerRoleName(user.GetPartitionId()));

    /// <summary>
    /// Determines whether the specified user is a partition administrator.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>True if the user is a partition administrator; otherwise, false.</returns>
    public static bool IsPartitionAdministrator(this ClaimsPrincipal user)
        => (user.GetRoles() ?? [])
        .Any(p => p == GlobalAdministratorRole);

    /// <summary>
    /// Determines whether the specified user is a reader.
    /// </summary>
    /// <param name="user">The user principal.</param>
    /// <returns>True if the user is a reader; otherwise, false.</returns>
    public static bool IsReader(this ClaimsPrincipal user)
        => (user.GetRoles() ?? [])
            .Any(p => p == ReaderRoleName(user.GetPartitionId()));

    /// <summary>
    /// Gets the owner role name for the specified partition ID.
    /// </summary>
    /// <param name="partitionId">The partition ID.</param>
    /// <returns>The owner role name.</returns>
    public static string OwnerRoleName(string partitionId)
                    => ApplicationClaimPrefix + partitionId + "-owner";

    public static string PartitionPrefix(string partitionId)
        => ApplicationClaimPrefix + partitionId + "-";

    public static IEnumerable<string> PartitionRoles(this ClaimsPrincipal user)
    {
        var partitionPrefix = PartitionPrefix(user.GetPartitionId());
        return user
            .GetRoles()
                .Where(p => p.StartsWith(partitionPrefix))
                .Select(p => p.Substring(partitionPrefix.Length));
    }

    /// <summary>
    /// Gets the reader role name for the specified partition ID.
    /// </summary>
    /// <param name="partitionId">The partition ID.</param>
    /// <returns>The reader role name.</returns>
    public static string ReaderRoleName(string partitionId)
        => ApplicationClaimPrefix + partitionId + "-reader";
}