namespace Hexalith.Application.Modules.Services;

using Hexalith.Application.Modules.Models;

/// <summary>
/// Represents the module management service.
/// </summary>
public interface IModuleManagementService
{
    /// <summary>
    /// Retrieves the module information asynchronously.
    /// </summary>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task<ModuleInformation> GetApplicationInformationAsync();

    /// <summary>
    /// Retrieves the module information asynchronously by name.
    /// </summary>
    /// <param name="name">The name of the module.</param>
    /// <returns>The task representing the asynchronous operation.</returns>
    Task<ModuleInformation> GetModuleInformationAsync(string name);
}