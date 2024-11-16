namespace Hexalith.Application.Modules.Services;

/// <summary>
/// Represents an module service.
/// </summary>
public interface IModuleService
{
    /// <summary>
    /// Gets the description of the module.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets a value indicating whether the module is an application module.
    /// </summary>
    public bool IsApplicationModule { get; }

    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the version of the module.
    /// </summary>
    public string Version { get; }
}