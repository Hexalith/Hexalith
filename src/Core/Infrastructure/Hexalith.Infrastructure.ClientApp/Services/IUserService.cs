// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.ClientApp.Services;

using System.Threading.Tasks;

/// <summary>
/// Represents a service for managing user sessions.
/// </summary>
public interface IUserService
{
    Task<string> GetUserIdAsync(CancellationToken cancellationToken);
}