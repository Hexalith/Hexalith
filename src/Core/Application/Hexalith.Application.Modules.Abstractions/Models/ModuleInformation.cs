namespace Hexalith.Application.Modules.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents the module information.
/// </summary>
[DataContract]
public record ModuleInformation(

    /// <summary>
    /// Gets or sets the name of the module.
    /// </summary>
    [property: DataMember(Order = 1)]
    string Name,

    /// <summary>
    /// Gets or sets the description of the module.
    /// </summary>
    [property: DataMember(Order = 2)]
    string Description,

    /// <summary>
    /// Gets or sets the version of the module.
    /// </summary>
    [property: DataMember(Order = 3)]
    string Version,

    /// <summary>
    /// Gets or sets the version of the module.
    /// </summary>
    [property: DataMember(Order = 4)]
    bool IsApplicationModule)
{
}