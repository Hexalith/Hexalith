// <copyright file="DummyServerService.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

public record DummyServerService(string Id)
{
    public DummyServerService()
    : this("Server")
    {
    }
}